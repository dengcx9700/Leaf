using Ao.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Leaf.Model
{
    /// <summary>
    /// 评论
    /// </summary>
    public class ArticleComment
    {
        public ArticleComment()
        {
            Id = ObjectId.GenerateNewId();
            CreateTime = TimeHelper.GetTimestamp();
            PrevId = ObjectId.Empty;
        }
        [BsonId]
        public ObjectId Id { get; set; }
        /// <summary>
        /// 上一个id
        /// </summary>
        public ObjectId PrevId { get; set; }
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
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }
    }
}
