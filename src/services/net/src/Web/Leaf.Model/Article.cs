using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leaf.Model
{
    public class Article : DbModelBase
    {
        /// <summary>
        /// 文档id
        /// </summary>
        [Key]
        [StringLength(128)]
        public string DocumentId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        public LUser User { get; set; }
    }
}
