using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.DataFlow
{
    public abstract class DataFlowContextBase<TModel>
    {
        public TModel Model { get; }
    }
}
