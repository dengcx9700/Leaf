using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.Command.Attributes
{
    /// <summary>
    /// 表示此方法不是命令
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NotCommandAttribute : Attribute
    {
    }
}
