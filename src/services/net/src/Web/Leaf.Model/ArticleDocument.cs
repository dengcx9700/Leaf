using Ao.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Model
{
    public class ArticleDocumentTrunk
    {
        public ArticleDocumentTrunk()
        {
            Id = ObjectId.GenerateNewId();
            Tags = Array.Empty<string>();
            CreateTime = TimeHelper.GetTimestamp();
            Enable = true;
        }

        [BsonId]
        public ObjectId Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [BsonRequired]
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descript { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [BsonIgnore]
        public string UserName { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 点赞人数
        /// </summary>
        public ulong LikeCount { get; set; }
        /// <summary>
        /// 阅读人数
        /// </summary>
        public ulong ReadCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable { get; set; }
    }
    public class ArticleDocument:ArticleDocumentTrunk
    {
        /// <summary>
        /// 评论集合
        /// </summary>
        public List<ArticleComment> Comments { get; set; }
    }
}
