using Leaf.Model;
using Leaf.Model.ResponseEntity;
using Microsoft.Extensions.Logging;
using RedLockNet;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Leaf.Data
{
    public static class ApiRunServiceExtensions
    {
        public static readonly string HeaderLocker = "dislocker";
        public static Task<IRedLock> GetLockerAsync(this IApiRunService service,params object[] keys)
        {
            var key = KeyGen.Gen(keys);
            return GetLockerAsync(service, key);
        }
        public static Task<IRedLock> GetLockerAsync(this IApiRunService service, string key)
        {
            return service.LockFactory.CreateLockAsync(HeaderLocker+key, TimeSpan.FromMilliseconds(service.ApiConfiguration.RedLockOuttimeMillSec));
        }
        public static T SystemBusy<T>(this IApiRunService runService)
             where T : Result, new()
        {
            return new T
            {
                Msg = runService.LangService["Response.Error.SystemBusy"]
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T HandleException<T>(this IApiRunService runService,Exception ex,ILogger logger)
            where T:Result,new()
        {
            logger.LogError(ex.ToString());
            return new T
            {
                Msg = runService.LangService["Response.Error.Exception"]
            };
        }
        public static async ValueTask<BytesValue<T>?> GetCacheAsync<T>(this IApiRunService runService,string resourceKey,string notFountKey)
            where T:Result,new()
        {
            var cacheBytes = await runService.Cache.GetAsync(resourceKey);
            if (cacheBytes != null)
            {
                if (cacheBytes.Length == 0)
                {
                    return new BytesValue<T>(new T
                    {
                        Msg = runService.LangService[notFountKey]
                    });
                }
                else
                {
                    //发现热评?
                    return new BytesValue<T>(cacheBytes);
                }
            }
            return null;
        }
        public static HitResult Hit(this IApiRunService runService, HotFinder hotFinder, string key, string hotKey)
        {
            var ttlHot = runService.RedisClient.PTtl(hotKey);
            var ttlValue = runService.RedisClient.PTtl(key);
            if (ttlHot > 0 && ttlValue > 0 && hotFinder != null)
            {
                if (hotFinder.HitTriggers != null)
                {
                    var count = runService.RedisClient.Incr(hotKey);
                    var i = 0;
                    for (; i < hotFinder.HitTriggers.Length; i++)
                    {
                        var trigger = hotFinder.HitTriggers[i];
                        if (count >= trigger.NeedHitCount)
                        {
                            ttlHot += trigger.IncMillSec;
                        }
                        else
                        {
                            break;
                        }
                    }
                    runService.RedisClient.PExpire(hotKey, ttlHot);
                    runService.RedisClient.PExpire(key, ttlHot);
                    return new HitResult(i == hotFinder.HitTriggers.Length,i, ttlHot, ttlHot);
                }
            }
            return HitResult.Empty;
        }
    }
    public struct HitResult
    {
        public static readonly HitResult Empty = new HitResult(false,-1, -1, -1);
        public bool AllHit;

        public int HitCount;

        public long OldTtl;

        public long NewTtl;

        public HitResult(bool allHit, int hitCount, long oldTtl, long newTtl)
        {
            AllHit = allHit;
            HitCount = hitCount;
            OldTtl = oldTtl;
            NewTtl = newTtl;
        }
    }
}
