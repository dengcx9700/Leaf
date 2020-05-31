using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Data
{
    public class MongoDbOptionsItem
    {
        /// <summary>
        /// 数据库名
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// 集合名字
        /// </summary>
        public string CollectionName { get; set; }
    }
    public class MongoDbOptions
    {
        /// <summary>
        /// 文章
        /// </summary>
        public MongoDbOptionsItem Article { get; set; }
    }
}
