using Ao.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Model
{
    public abstract class DbModelBase
    {
        public DbModelBase()
        {
            CreateTime = TimeHelper.GetTimestamp();
            Enable = true;
        }
        public long CreateTime { get; set; }

        public bool Enable { get; set; }
    }
}
