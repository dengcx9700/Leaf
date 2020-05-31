using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Model.ResponseEntity
{
    public class Result
    {
        public static readonly Result SucceedResult = new Result { Succeed = true };
        public string Msg { get; set; }

        public bool Succeed { get; set; }

    }
}
