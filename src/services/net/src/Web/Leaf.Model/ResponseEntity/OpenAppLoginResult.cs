using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Model.ResponseEntity
{
    public class OpenAppLoginResult : Result
    {
        public string AppSession { get; set; }

        public long LifeTime { get; set; }
    }
}
