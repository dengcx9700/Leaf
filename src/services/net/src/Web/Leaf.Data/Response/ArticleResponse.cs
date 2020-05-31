using Ao.Core;
using DnsClient.Internal;
using Leaf.Model;
using Leaf.Model.ResponseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using ServiceStack;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;


namespace Leaf.Data.Response
{
    public class ArticleResponse
    {
        public readonly static string Header = "atc";
        public readonly static string HeaderCreate = Header + ".create";
        public readonly static string HeaderRemove = Header + ".remove";
        public readonly static string HeaderFind = Header + ".find";
        public readonly static string HeaderGet = Header + ".get";
        public readonly static string HeaderHotFind = Header + ".hotfind";
        public readonly static string HeaderAtricleHotFind = HeaderHotFind + "_article";
        public readonly static string HeaderAtricleHotted = Header + ".hotted_article";
        public readonly static string HeaderGetComment = Header + ".comment";
        public readonly static string HeaderCreateComment = Header + ".createcomment";
        public readonly static string HeaderRemoveComment = Header + ".removecomment";

        private readonly MongoDbOptions mongoDbOptions;
        private readonly ILogger<ArticleResponse> logger;
        private readonly IApiRunService runService;
        private readonly HotDataFinderSetting hotDataFinderSetting;

        public ArticleResponse(MongoDbOptions mongoDbOptions, ILogger<ArticleResponse> logger,
            IApiRunService runService, HotDataFinderSetting hotDataFinderSetting)
        {
            this.mongoDbOptions = mongoDbOptions;
            this.logger = logger;
            this.runService = runService;
            this.hotDataFinderSetting = hotDataFinderSetting;
        }

        public async ValueTask<Result> CreateAsync(long userId, string body, string[] tags)
        {
            using var locker = await runService.GetLockerAsync(HeaderCreate, userId);
            if (locker.IsAcquired)
            {
                try
                {
                    var session = await runService.MongoClient.StartSessionAsync();
                    var db = runService.MongoClient.GetDatabase(mongoDbOptions.Article.DatabaseName);
                    var coll = db.GetCollection<ArticleDocument>(mongoDbOptions.Article.CollectionName);
                    using var tans = await runService.DbContext.Database.BeginTransactionAsync();
                    var doc = new ArticleDocument
                    {
                        Body = body,
                        UserId = userId,
                        Tags = tags
                    };
                    await coll.InsertOneAsync(session, doc);
                    var article = new Article
                    {
                        DocumentId = doc.Id.ToString(),
                        UserId = userId
                    };
                    runService.DbContext.Articles.Add(article);
                    var res = await runService.DbContext.SaveChangesAsync();
                    if (res == 1)
                    {
                        await session.CommitTransactionAsync();
                        await tans.CommitAsync();
                        return Result.SucceedResult;
                    }
                    return new Result
                    {
                        Msg = runService.LangService["Response.Error.SaveFail"]
                    };
                }
                catch (Exception ex)
                {
                    return runService.HandleException<Result>(ex, logger);
                }
            }
            return runService.SystemBusy<Result>();
        }
        public async ValueTask<Result> RemoveAsync(long userId, string articleId)
        {
            var key = HeaderRemove + userId + articleId;
            using var locker = await runService.GetLockerAsync(key);
            if (locker.IsAcquired)
            {
                var isRemoved = runService.RedisClient.ContainsKey(key);
                if (isRemoved)
                {
                    return new Result
                    {
                        Msg = runService.LangService["Response.Article.Error.NotFound"]
                    };
                }
                try
                {
                    var count = await runService.DbContext.Articles.AsNoTracking()
                        .Where(a => a.UserId == userId && a.DocumentId == articleId)
                        .Take(1)
                        .DeleteFromQueryAsync();
                    runService.RedisClient.Add(key, false, TimeSpan.FromSeconds(2));
                    if (count == 1)
                    {
                        var removeKey = KeyGen.Gen(HeaderGet, articleId);
                        runService.RedisClient.Remove(removeKey);
                        return Result.SucceedResult;
                    }
                    return new Result
                    {
                        Msg = runService.LangService["Response.Error.RemoveFail"]
                    };
                }
                catch (Exception ex)
                {
                    return runService.HandleException<Result>(ex, logger);
                }
            }
            return runService.SystemBusy<Result>();
        }
        public async ValueTask<BytesValue<Result>> RemoveCommentAsync(long userId,string articleId,string commentId)
        {
            try
            {
                var key = KeyGen.Gen(HeaderRemoveComment, articleId, commentId);
                var cacheRes = await runService.GetCacheAsync<Result>(key, "Response.Article.Error.CommentNotFound");
                if (cacheRes!=null)
                {
                    return cacheRes.Value;
                }
                using var locker = await runService.GetLockerAsync(key);
                if (locker.IsAcquired)
                {
                    var db = runService.MongoClient.GetDatabase(mongoDbOptions.Article.DatabaseName);
                    var coll = db.GetCollection<ArticleDocument>(mongoDbOptions.Article.CollectionName);
                    var objId = new ObjectId(articleId);
                    var commId = new ObjectId(commentId);
                    var foundRes = await HasArticleAsync<Result>(coll, key, objId);
                    if (foundRes!=null)
                    {
                        return foundRes.Value;
                    }

                    var updef = new UpdateDefinitionBuilder<ArticleDocument>();
                    var def=updef.PullFilter(x => x.Comments, x => x.Id == commId);

                    var res = await coll.UpdateOneAsync(x => x.Id == objId && x.UserId == userId, def);
                    return new BytesValue<Result>(new Result
                    {
                        Succeed = res.ModifiedCount > 0
                    });
                }
                return new BytesValue<Result>(runService.SystemBusy<Result>());
            }
            catch (Exception ex)
            {
                return new BytesValue<Result>(runService.HandleException<Result>(ex, logger));
            }
        }
        private async ValueTask<BytesValue<T>?> HasArticleAsync<T>(IMongoCollection<ArticleDocument> coll,
            string resourceKey,ObjectId objectId)
        {
            var hasArticle =await coll.AsQueryable()
                       .Where(x => x.Id == objectId)
                       .Take(1)
                       .AnyAsync();
            if (!hasArticle)
            {
                var notExist = new Result
                {
                    Msg = runService.LangService["Response.Article.Error.NotFound"]
                };
                var bytes = JsonSerializer.SerializeToUtf8Bytes(notExist);

                await runService.Cache.SetAsync(resourceKey, bytes, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMilliseconds(runService.ApiConfiguration.NotExistCacheMillTime)
                });

                return new BytesValue<T>(bytes);
            }
            return null;
        }
        public async ValueTask<BytesValue<PageableCacherEntityResult<ArticleDocumentTrunk[]>>> FindAsync(string findKey,string lastArticleId,int skip,int take)
        {
            try
            {
                var key = KeyGen.Gen(HeaderFind, findKey);
                var cacheRes = await runService.GetCacheAsync<PageableCacherEntityResult<ArticleDocumentTrunk[]>>(key, null);
                if (cacheRes!=null)
                {
                    return cacheRes.Value;
                }
                using var locker = await runService.GetLockerAsync(key);
                if (locker.IsAcquired)
                {
                    cacheRes = await runService.GetCacheAsync<PageableCacherEntityResult<ArticleDocumentTrunk[]>>(key, null);
                    if (cacheRes != null)
                    {
                        return cacheRes.Value;
                    }
                    var db = runService.MongoClient.GetDatabase(mongoDbOptions.Article.DatabaseName);
                    var coll = db.GetCollection<ArticleDocument>(mongoDbOptions.Article.CollectionName);
                    var lastId=ObjectId.Empty;
                    var hasLast = false;
                    if (!string.IsNullOrEmpty(lastArticleId))
                    {
                        hasLast=ObjectId.TryParse(lastArticleId, out lastId);
                    }

                    IQueryable<ArticleDocument> query = coll.AsQueryable();

                    if (hasLast)
                    {
                        query = query.Where(d => d.Id == lastId);
                    }
                    query = query.Where(d => d.Title.ToLower().Contains(findKey) || d.Body.ToLower().Contains(findKey)
                          || d.Tags.Any(t => t.Contains(findKey)));

                    if (!hasLast)
                    {
                        query = query.Skip(skip);
                    }
                    query = query.Take(take);

                    var count = await query.LongCountAsync();

                    var datas = await query.ToArrayAsync();

                    var value = new PageableCacherEntityResult<ArticleDocumentTrunk[]>
                    {
                        CacheTime = TimeHelper.GetTimestamp(),
                        Take = take,
                        Skip = skip,
                        Total = count,
                        Entity = datas,
                        Succeed = true
                    };

                    var bytes = JsonSerializer.SerializeToUtf8Bytes(value);

                    await runService.Cache.SetAsync(key, bytes,new DistributedCacheEntryOptions
                    {
                        SlidingExpiration=TimeSpan.FromMilliseconds(runService.ApiConfiguration.ArticleFindCacheMillTime)
                    });

                    return new BytesValue<PageableCacherEntityResult<ArticleDocumentTrunk[]>>(bytes);
                }
                return new BytesValue<PageableCacherEntityResult<ArticleDocumentTrunk[]>>(runService.SystemBusy<PageableCacherEntityResult<ArticleDocumentTrunk[]>>());
            }
            catch (Exception ex)
            {
                return new BytesValue<PageableCacherEntityResult<ArticleDocumentTrunk[]>>(runService.HandleException<PageableCacherEntityResult<ArticleDocumentTrunk[]>>(ex, logger));
            }
        }
        public async ValueTask<BytesValue<Result>> CreateCommentAsync(long userId,string articleId,
            string prevCommentId, string content)
        {
            try
            {
                var key = KeyGen.Gen(HeaderCreateComment, userId);
                var cacheResult = await runService.GetCacheAsync<Result>(key, "Response.Article.Error.NotFound");
                if (cacheResult!=null)
                {
                    return cacheResult.Value;
                }
                using var locker = await runService.GetLockerAsync(key);
                if (locker.IsAcquired)
                {
                    var db = runService.MongoClient.GetDatabase(mongoDbOptions.Article.DatabaseName);
                    var coll = db.GetCollection<ArticleDocument>(mongoDbOptions.Article.CollectionName);
                    var objId = ObjectId.Parse(articleId);
                    var prevId=ObjectId.Empty;

                    if (!string.IsNullOrEmpty(prevCommentId))
                    {
                        ObjectId.TryParse(prevCommentId, out prevId);
                    }
                    var foundRes = await HasArticleAsync<Result>(coll, key, objId);
                    if (foundRes!=null)
                    {
                        return foundRes.Value;
                    }

                    var setBuilder = new UpdateDefinitionBuilder<ArticleDocument>();

                    var set = setBuilder.Push(x => x.Comments, new ArticleComment
                    {
                        Content = content,
                        PrevId = prevId,
                        CreateTime = TimeHelper.GetTimestamp(),
                        UserId = userId
                    });

                    var res = await coll.UpdateOneAsync(x => x.Id == objId, set);
                    if (res.ModifiedCount==1)
                    {
                        return new BytesValue<Result>(new Result
                        {
                            Succeed = true
                        });
                    }
                    return new BytesValue<Result>(new Result
                    {
                        Msg=runService.LangService["Response.Article.Error.CommentFailt"]
                    });
                }
                return new BytesValue<Result>(runService.SystemBusy<Result>());
            }
            catch (Exception ex)
            {
                return new BytesValue<Result>(runService.HandleException<Result>(ex, logger));
            }
        }

        public async ValueTask<BytesValue<CacheableEntityResult<ArticleDocumentTrunk>>> GetAsync(string articleId)
        {
            try
            {
                var key = KeyGen.Gen(HeaderGet, articleId);
                void Hit()
                {
                    var hotKey = KeyGen.Gen(HeaderAtricleHotFind, articleId);
                    var hitResult = runService.Hit(hotDataFinderSetting.ArticleHotFinder, key, hotKey);
                    if (hitResult.AllHit)
                    {
                        runService.RedisClient.Set(HeaderAtricleHotted, TimeHelper.GetTimestamp(),
                            TimeSpan.FromMilliseconds(hotDataFinderSetting.ArticleHotFinder.AllHitIncMillSec + hitResult.NewTtl));
                        logger.LogInformation("Article id:{0} hit all, set resource time useable:{1}ms", articleId, hitResult.NewTtl);
                    }
                }
                var cacheRes = await runService.GetCacheAsync<CacheableEntityResult<ArticleDocumentTrunk>>(key, "Response.Article.Error.NotFound");
                if (cacheRes != null)
                {
                    Hit();
                    return cacheRes.Value;
                }
                using var locker = await runService.GetLockerAsync(key);
                if (locker.IsAcquired)
                {
                    var res = await runService.GetCacheAsync<CacheableEntityResult<ArticleDocumentTrunk>>(key, "Response.Article.Error.NotFound");
                    if (res != null)
                    {
                        Hit();
                        return res.Value;
                    }
                    var db = runService.MongoClient.GetDatabase(mongoDbOptions.Article.DatabaseName);
                    var coll = db.GetCollection<ArticleDocument>(mongoDbOptions.Article.CollectionName);
                    var targetId = new ObjectId(articleId);
                    var foundRes = await HasArticleAsync<CacheableEntityResult<ArticleDocumentTrunk>>(coll, key, targetId);
                    if (foundRes!=null)
                    {
                        return foundRes.Value;
                    }
                    var cour = await coll.Find(x => x.Id == targetId)
                        .Project(doc => new ArticleDocumentTrunk
                        {
                            CreateTime = doc.CreateTime,
                            Body = doc.Body,
                            Enable = doc.Enable,
                            Id = doc.Id,
                            ReadCount = doc.ReadCount,
                            LikeCount = doc.LikeCount,
                            Tags = doc.Tags,
                            UserId = doc.UserId
                        })
                        .Limit(1)
                        .FirstOrDefaultAsync();
                    if (cour == null)
                    {
                        var value = new CacheableEntityResult<ArticleDocumentTrunk>
                        {
                            CacheTime = TimeHelper.GetTimestamp(),
                            Entity = cour,
                            Succeed = true
                        };
                        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
                        var startTimeMillSec = hotDataFinderSetting.ArticleHotFinder.StartMillSec;
                        await runService.Cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromMilliseconds(startTimeMillSec)
                        });
                        var hotKey = KeyGen.Gen(HeaderAtricleHotFind, articleId);
                        var setHot = runService.RedisClient.Set(hotKey, 1, TimeSpan.FromMilliseconds(startTimeMillSec));
                        if (!setHot)
                        {
                            logger.LogWarning("设置热点寻找失败:{0}", hotKey);
                        }
                        return new BytesValue<CacheableEntityResult<ArticleDocumentTrunk>>(bytes);
                    }
                    await runService.Cache.SetAsync(key, Array.Empty<byte>(), new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMilliseconds(hotDataFinderSetting.ArticleHotFinder.StartMillSec)
                    });
                    return new BytesValue<CacheableEntityResult<ArticleDocumentTrunk>>(new CacheableEntityResult<ArticleDocumentTrunk>
                    {
                        Msg = runService.LangService["Response.Article.Error.NotFound"]
                    });
                }
                return new BytesValue<CacheableEntityResult<ArticleDocumentTrunk>>(runService.SystemBusy<CacheableEntityResult<ArticleDocumentTrunk>>());
            }
            catch (Exception ex)
            {
                return new BytesValue<CacheableEntityResult<ArticleDocumentTrunk>>(runService.HandleException<CacheableEntityResult<ArticleDocumentTrunk>>(ex, logger));
            }
        }

        public async ValueTask<BytesValue<PageableCacherEntityResult<List<ArticleComment>>>> GetCommentAsync(string articleId, string lastArticleId, int skip, int take)
        {
            try
            {
                var key = KeyGen.Gen(HeaderGetComment, articleId);
                var cacheRes = await runService.GetCacheAsync<PageableCacherEntityResult<List<ArticleComment>>>(key, "Response.Article.Error.NotFound");
                if (cacheRes != null)
                {
                    return cacheRes.Value;
                }
                using var locker = await runService.GetLockerAsync(key);
                if (locker.IsAcquired)
                {
                    cacheRes = await runService.GetCacheAsync<PageableCacherEntityResult<List<ArticleComment>>>(key, "Response.Article.Error.NotFound");
                    if (cacheRes != null)
                    {
                        return cacheRes.Value;
                    }
                    var db = runService.MongoClient.GetDatabase(mongoDbOptions.Article.DatabaseName);
                    var coll = db.GetCollection<ArticleDocument>(mongoDbOptions.Article.CollectionName);
                    var targetId = new ObjectId(articleId);
                    var lastId = ObjectId.Empty;

                    if (!string.IsNullOrEmpty(lastArticleId))
                    {
                        ObjectId.TryParse(lastArticleId, out lastId);
                    }

                    var query = coll.AsQueryable()
                        .Where(x => x.Id == targetId);

                    //看一下有没有这个文章
                    var hasArticle = await query.AnyAsync();
                    if (!hasArticle)
                    {
                        await runService.Cache.SetAsync(key, Array.Empty<byte>(), new DistributedCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromMilliseconds(runService.ApiConfiguration.CommentCacheMillTime)
                        });
                        return new BytesValue<PageableCacherEntityResult<List<ArticleComment>>>(new PageableCacherEntityResult<List<ArticleComment>>
                        {
                            Msg = runService.LangService["Response.Article.Error.NotFound"]
                        });
                    }

                    var total = await query.Select(c => c.Comments.Count).FirstOrDefaultAsync();
                    IQueryable<IEnumerable<ArticleComment>> commQuery;
                    if (lastId != ObjectId.Empty)
                    {
                        commQuery = query.Select(x => x.Comments.Where(c => c.Id > lastId).Take(take));
                    }
                    else
                    {
                        commQuery = query.Select(x => x.Comments.Skip(skip).Take(take));
                    }

                    var datas = await commQuery
                        .Take(1)
                        .FirstOrDefaultAsync();

                    var value = new PageableCacherEntityResult<List<ArticleComment>>
                    {
                        CacheTime = TimeHelper.GetTimestamp(),
                        Entity = datas.ToList(),
                        Total = total,
                        Skip = skip,
                        Take = take,
                        Succeed = true
                    };

                    var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
                    await runService.Cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMilliseconds(runService.ApiConfiguration.CommentCacheMillTime)
                    });
                    return new BytesValue<PageableCacherEntityResult<List<ArticleComment>>>(bytes);
                }
                return new BytesValue<PageableCacherEntityResult<List<ArticleComment>>>(runService.SystemBusy<PageableCacherEntityResult<List<ArticleComment>>>());
            }
            catch (Exception ex)
            {
                return new BytesValue<PageableCacherEntityResult<List<ArticleComment>>>(runService.HandleException<PageableCacherEntityResult<List<ArticleComment>>>(ex, logger));
            }
        }

    }
}
