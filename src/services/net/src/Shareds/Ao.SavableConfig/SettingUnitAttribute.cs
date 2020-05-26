using System;
using System.Collections.Generic;
using System.Text;

namespace Ao.SavableConfig
{
    /// <summary>
    /// 表示这是一个设置的插件单元,被标记的类可以在设置中发现
    /// <para>
    /// <inheritdoc/>
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SettingUnitAttribute : Attribute
    {
    }
}
