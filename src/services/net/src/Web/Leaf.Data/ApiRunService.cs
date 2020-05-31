using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RedLockNet;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Ao.Lang;
using MongoDB.Driver;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace Leaf.Data
{
    public class ApiRunService : IApiRunService
    {
        private readonly Lazy<LeafDbContext> dbContext;
        private readonly Lazy<IDistributedCache> cache;
        private readonly Lazy<MongoClient> mongoClient;
        private readonly Lazy<RedisClient> redisClient;

        public ApiRunService(IServiceProvider serviceProvider)
        {
            dbContext = new Lazy<LeafDbContext>(() => serviceProvider.GetService<LeafDbContext>());
            cache = new Lazy<IDistributedCache>(() => serviceProvider.GetService<IDistributedCache>());
            LockFactory = serviceProvider.GetService<IDistributedLockFactory>();
            ApiConfiguration = serviceProvider.GetService<IApiConfiguration>();
            mongoClient = new Lazy<MongoClient>(() => serviceProvider.GetService<MongoClient>());
            redisClient = new Lazy<RedisClient>(() => serviceProvider.GetService<RedisClient>());
            LangService = serviceProvider.GetService<ILanguageService>();
        }

        public LeafDbContext DbContext => dbContext.Value;
        public IDistributedCache Cache => cache.Value;
        public IDistributedLockFactory LockFactory { get; }

        public ILanguageService LangService { get; }

        public MongoClient MongoClient => mongoClient.Value;

        public IApiConfiguration ApiConfiguration { get; }

        public RedisClient RedisClient => redisClient.Value;

        public async ValueTask DisposeAsync()
        {
            if (dbContext.IsValueCreated)
            {
                await dbContext.Value.DisposeAsync();
            }

            if (redisClient.IsValueCreated)
            {
                redisClient.Value.Dispose();
            }
        }
    }
}
