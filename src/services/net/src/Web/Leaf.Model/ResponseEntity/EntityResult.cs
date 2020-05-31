using System;
using System.Collections.Generic;
using System.Text;

namespace Leaf.Model.ResponseEntity
{
    public class EntityResult<T>:Result
    {
        public T Entity { get; set; }
    }
}
