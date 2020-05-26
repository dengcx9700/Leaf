using Ao.Wpf.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Ao.Wpf.ViewModels
{
    /// <summary>
    /// 引用类型的数组适配
    /// </summary>
    public class ClassArrayViewAdapter : IArrayViewAdapter
    {
        private readonly Dictionary<Type, AoMemberNewer<object>> newerCacher = new Dictionary<Type, AoMemberNewer<object>>();
        
        public object MakeObject(ArrayViewAdapterContext context)
        {
            if (!newerCacher.TryGetValue(context.GenericType, out var newer))
            {
                newer = ReflectionHelper.GetNewer(context.GenericType);
                newerCacher.Add(context.GenericType, newer);
            }
            return newer();
        }
        public UIElement GetView(object obj,ArrayViewAdapterContext context)
        {
            ThrowIfValueType(context.GenericType);
            var exp = new AoExpander ();
            var items = new ItemsControl
            {
                ItemContainerStyle = new Style(typeof(FrameworkElement))
                {
                    Setters =
                    {
                        new  Setter(FrameworkElement.MarginProperty,new Thickness(6))
                    }
                }
            };
            exp.Content = items;
            if (obj is IEnumerable list)
            {
                var propInfo = new AoAnalizedListPropertyItem(list, "Value");
                ThrowIfNotGenericType(list.GetType());
                var genType = list.GetType().GenericTypeArguments[0];
                exp.Header = context.Context.ViewBuilder.StringProvider?.GetString(genType.Name)?? genType.Name;//TODO:更改
                var ctx = new ArrayViewAdapterContext(list, genType, propInfo, context.Context);
                foreach (var item in list)
                {
                    var view = GetView(item, ctx);
                    items.Items.Add(view);
                }
            }
            else
            {
                var objType = obj.GetType();
                exp.Header = objType.Name;
                if (!objType.IsValueType || objType.IsPrimitive || objType.IsEnum)
                {
                    var props = AoAnalizer.Default.Analize(obj, false);
                    foreach (var item in props.MemberItems)
                    {
                        var ui = context.Context.ViewBuilder.Build(context.Context.Vm, item);
                        items.Items.Add(ui);
                    }
                }
            }
            return exp;
        }
        private static void ThrowIfValueType(Type type)
        {
            if (type.IsValueType)
            {
                throw new NotSupportedException("不支持值类型泛型");
            }
        }
        private static void ThrowIfNotGenericType(Type type)
        {
            if (type.GenericTypeArguments.Length==0)
            {
                throw new NotSupportedException("不支持非泛型");
            }
        }
    }
}
