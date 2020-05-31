using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leaf.Model
{
    public class LUser:IdentityUser<long>
    {
        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(256)]
        public string Portrait { get; set; }
    }
}
