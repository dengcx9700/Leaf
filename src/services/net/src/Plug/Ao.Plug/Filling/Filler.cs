using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Linq;

namespace Ao.Plug.Filling
{
    /// <summary>
    /// 填充器
    /// </summary>
    public class Filler
    {
        /// <summary>
        /// 初始化<see cref="Filler"/>
        /// </summary>
        /// <param name="plugLook"><inheritdoc cref="PlugLook"/></param>
        /// <param name="target"><inheritdoc cref="Target"/></param>
        public Filler(IPlugLookup plugLook, object target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            if (!target.GetType().IsClass)
            {
                throw new ArgumentException("目标只能是类");
            }
            PlugLook = plugLook ?? throw new ArgumentNullException(nameof(plugLook));
            Target = target;
        }
        /// <summary>
        /// 插件查看器
        /// </summary>
        public IPlugLookup PlugLook { get; }
        /// <summary>
        /// 目标对象，此对象必须是类
        /// </summary>
        public object Target { get; }
        /// <summary>
        /// 执行填充
        /// </summary>
        /// <returns></returns>
        public FillContext[] Fill()
        {
            var includeContexts = new List<FillContext>();
            var props = Target.GetType()
                              .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                              .ToArray();
            foreach (var item in props)
            {
                var ctxs=FillOne(item);
                includeContexts.AddRange(ctxs);
            }
            return includeContexts.ToArray();
        }
        protected virtual FillContext[] FillOne(PropertyInfo propertyInfo)
        {
            var attr = propertyInfo.GetCustomAttributes<FillAttribute>().ToArray();
            var includeContext = new List<FillContext>();
            if (attr.Length != 0 && propertyInfo.PropertyType.GetInterface(typeof(ICollection).FullName) != null)
            {
                var collection = propertyInfo.GetValue(Target) as IList;
                foreach (var item in attr)
                {
                    var objs=PlugLook.GetInstances(t => item.RefType.IsAssignableFrom(t.TargetType));
                    var context = new FillContext
                    {
                        Property = propertyInfo,
                        Target = Target,
                        TargetList = collection,
                        Values = objs
                    };
                    includeContext.Add(context);
                    item.PutIn(context);
                }
            }
            return includeContext.ToArray();
        }
    }
}
