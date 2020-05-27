using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leaf.Model
{
    public class OpenApp:DbModelBase
    {
        [Required]
        public string AppKey { get; set; }
        
        [Required]
        public string AppSecret { get; set; }


    }
}
