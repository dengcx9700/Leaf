using Ao.Shared.ForView;
using System;
using System.Windows;

namespace Ao.Wpf.ViewModels
{
    public class ArrayViewAdapterContext
    {
        public ArrayViewAdapterContext(object array, Type genericType, AoAnalizedPropertyItemBase item, ViewBuildContext<UIElement> context)
        {
            Array = array;
            GenericType = genericType;
            Item = item;
            Context = context;
        }

        /// <summary>
        /// 数组对象
        /// </summary>
        public object Array { get; }
        /// <summary>
        /// 数组的泛型类型
        /// </summary>
        public Type GenericType { get; }
        /// <summary>
        /// 源
        /// </summary>
        public AoAnalizedPropertyItemBase Item { get; }
        /// <summary>
        /// 视图生成上下文
        /// </summary>
        public ViewBuildContext<UIElement> Context { get; }
    }
}
