using Ao.Core;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leaf.Model
{
    public class OpenApp:DbModelBase
    {
        [Key]
        [Required]
        public string AppKey { get; set; }
        
        [Required]
        public string AppSecret { get; set; }

        public long? EndTime { get; set; }

        [Required]
        public string Platform { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        [Required]
        public string Version { get; set; }

        public static OpenApp MakeApp(string platform,string version,long? endTime)
        {
            var appkey = Guid.NewGuid().ToString().Replace("-", string.Empty);
            var guid = Guid.NewGuid().ToString();
            var secret = Md5Helper.GetMd5(guid+appkey, Md5LengthType.U32);
            return new OpenApp
            {
                AppKey = appkey,
                AppSecret = secret,
                EndTime = endTime,
                Version = version,
                Platform = platform
            };
        }
    }
}
