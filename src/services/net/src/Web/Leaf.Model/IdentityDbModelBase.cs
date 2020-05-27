using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leaf.Model
{
    public abstract class IdentityDbModelBase : DbModelBase
    {
        [Key]
        public long Id { get; set; }
    }
}
