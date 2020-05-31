using Ao.Lang;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using RedLockNet;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Data
{
    /// <summary>
    /// api服务
    /// </summary>
    public interface IApiRunService:IAsyncDisposable
    {
        /// <summary>
        /// 语言服务
        /// </summary>
        ILanguageService LangService { get; }
        /// <summary>
        /// 缓存服务
        /// </summary>
        IDistributedCache Cache { get; }
        /// <summary>
        /// 芒果db客户端
        /// </summary>
        MongoClient MongoClient { get; }
        /// <summary>
        /// api设置
        /// </summary>
        IApiConfiguration ApiConfiguration { get; }
        /// <summary>
        /// redis客户端
        /// </summary>
        RedisClient RedisClient { get; }

        /// <summary>
        /// 数据库
        /// </summary>
        LeafDbContext DbContext { get; }
        /// <summary>
        /// 分布式锁
        /// </summary>
        IDistributedLockFactory LockFactory { get; }
    }
}
