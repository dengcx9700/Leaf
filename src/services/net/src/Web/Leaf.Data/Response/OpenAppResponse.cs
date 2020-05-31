using Ao.Core;
using Ao.Lang;
using Leaf.Model.ResponseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaf.Data.Response
{
    public class OpenAppResponse
    {
        public static readonly TimeSpan AppLifeTime = TimeSpan.FromHours(12);

        private readonly ILogger<OpenAppResponse> logger;
        private readonly IApiRunService runService;

        /// <summary>
        /// app登录
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="platform"></param>
        /// <param name="version"></param>
        /// <param name="timestamp">时间戳,毫秒数</param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public async ValueTask<OpenAppLoginResult> LoginAsync(string appKey, string platform, string version, long timestamp, string sign)
        {
            if (string.IsNullOrEmpty(appKey) || string.IsNullOrEmpty(platform) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(sign))
            {
#if DEBUG
                logger.LogInformation("AppKey:{0},Platform:{1},Version:{2},Timestamp:{3},Sign:{4}", appKey, platform, version, timestamp, sign);
#endif
                return new OpenAppLoginResult { Msg = runService.LangService["Response.Error.HasErrorParams"] };
            }
            var now = TimeHelper.GetTimestamp(DateTime.Now);
            var timeSub = now - timestamp;
            var apiMillSec = runService.ApiConfiguration.ApiOutTimeMillSec;
            if (timeSub < -apiMillSec || timeSub > apiMillSec)
            {
                return new OpenAppLoginResult { Msg = runService.LangService["Response.Error.RequestFuture"] };
            }
            if (timeSub > runService.ApiConfiguration.ApiOutTimeMillSec)
            {
                return new OpenAppLoginResult { Msg = runService.LangService["Response.Error.RequestOutTime"] };
            }
            var appKeyKey = AppValidater.MakeAppKeyKey(appKey);
            var secert = await runService.Cache.GetStringAsync(appKeyKey);
            if (secert == AppValidater.FaileAppKey)
            {
                return new OpenAppLoginResult
                {
                    Msg = runService.LangService["Response.OpenApp.Error.ErrorAppKey"]
                };
            }
            var res = new OpenAppLoginResult();
            async Task GenSessionAsync()
            {
                var sessionKey = AppValidater.MakeSessionKey(appKey);
                var cacheSession = await runService.Cache.GetStringAsync(sessionKey);
                var createTimeKey = AppValidater.MakeSessionCreateTimeKey(appKey);
                if (!string.IsNullOrEmpty(cacheSession))
                {
                    res.AppSession = cacheSession;
                    res.Succeed = true;
                    var time = await runService.Cache.GetAsync(createTimeKey);
                    if (time != null)
                    {
                        var cacheTick = BitConverter.ToInt64(time);
                        logger.LogInformation("缓存中的时间为:{0}", cacheTick);
                        res.LifeTime = (long)TimeSpan.FromTicks(AppLifeTime.Ticks - (DateTime.Now.Ticks - cacheTick)).TotalMilliseconds;
                    }
                    return;
                }
                //签发session
                var session = Md5Helper.GetMd5(Guid.NewGuid().ToString(), Md5LengthType.U16);
                var createTime = BitConverter.GetBytes(DateTime.Now.Ticks);
#if DEBUG
                logger.LogInformation($"签发成功,session为{secert},时效为{AppLifeTime}");
#endif
                //写入缓存
                await runService.Cache.SetStringAsync(sessionKey, session, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = AppLifeTime
                });
                await runService.Cache.SetAsync(createTimeKey, createTime, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = AppLifeTime
                });
                res.AppSession = session;
                res.LifeTime = (long)AppLifeTime.TotalMilliseconds;
                res.Succeed = true;
            }
            using var locker = await runService.LockFactory.CreateLockAsync(appKeyKey, TimeSpan.FromMilliseconds(runService.ApiConfiguration.RedLockOuttimeMillSec));
            if (locker.IsAcquired)
            {
                if (string.IsNullOrEmpty(secert))//没有缓存
                {
#if DEBUG
                    logger.LogDebug($"Key:{appKey},Version:{version}");
#endif
                    //去找数据库
                    var appInfo = await runService.DbContext.OpenApps.AsNoTracking()
                                .Where(app => app.AppKey == appKey && app.Platform == platform && app.Version == version)
                                .Select(app => new { app.AppSecret, app.EndTime })
                                .SingleOrDefaultAsync();
                    if (appInfo == null || appInfo.EndTime != null && appInfo.EndTime.Value < DateTime.Now.Ticks)//找不到/过期了
                    {
                        //写入失败缓存
                        await runService.Cache.SetStringAsync(appKeyKey, AppValidater.FaileAppKey, new DistributedCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromSeconds(30)
                        });
                    }
                    else //找到了
                    {
                        //先放入缓存,保存12小时
                        await runService.Cache.SetStringAsync(appKeyKey, appInfo.AppSecret, new DistributedCacheEntryOptions
                        {
                            SlidingExpiration = AppLifeTime
                        });
                        secert = appInfo.AppSecret;
                    }
                }
                if (string.IsNullOrEmpty(secert))
                {
                    res.Msg = runService.LangService["Response.OpenApp.Error.AppKeyNotFound"];
                    return res;
                }
                //计算签名
                var signKey = appKey + timestamp + platform + version + secert;
                var mySign = Md5Helper.GetMd5(signKey, Md5LengthType.U16);
                //验证签名
                if (!string.Equals(sign, mySign, StringComparison.OrdinalIgnoreCase))
                {
#if DEBUG
                    res.Msg = $"签名匹配失败,请求签名{sign},计算后的签名{mySign},密匙为{secert},Key为{appKey}";
#endif
                }
                else
                {
                    await GenSessionAsync();
                }
            }
            else
            {
                res.Msg = runService.LangService["Response.Error.SystemBusy"];
            }
            return res;
        }

    }
}
