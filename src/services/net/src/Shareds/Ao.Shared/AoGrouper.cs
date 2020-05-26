using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
namespace Ao
{
    /// <summary>
    /// 表示分组器
    /// </summary>
    public class AoGrouper
    {
        /// <summary>
        /// 从分析结果进行分组，或获取属性的<see cref="AoGroupingAttribute"/>特性进行分组
        /// </summary>
        /// <param name="resual">分析结果</param>
        /// <returns></returns>
        public GroupedResual GroupResual(AoAnalizedResual resual)
        {
            var res = new GroupedResual();
            StepInResual(resual, res, (item, r) =>
             {
                 var gs = item.GetCustomAttributes<AoGroupingAttribute>();
                 foreach (var aga in gs)
                 {
                     if (r.Properties.TryGetValue(aga.ToString(), out var list))
                     {
                         list.Add(item);
                     }
                     else
                     {
                         r.Properties.Add(aga.ToString(), new List<AoAnalizedPropertyItemBase> { item });
                     }
                 }
                 return gs.Count();
             }, (item, r) =>
             {
                 var gs = item.Method.GetCustomAttributes<AoGroupingAttribute>();
                 foreach (var aga in gs)
                 {
                     if (r.Methods.TryGetValue(aga.ToString(), out var list))
                     {
                         list.Add(item);
                     }
                     else
                     {
                         r.Methods.Add(aga.ToString(), new List<AoAnalizedMethodItemBase> { item });
                     }
                 }
                 return gs.Count();
             });
            return res;
        }
        /// <summary>
        /// 从分析结果进行分组，或获取属性的类型名字进行分组
        /// </summary>
        /// <param name="resual"></param>
        /// <returns></returns>
        public GroupedResual GroupFromType(AoAnalizedResual resual)
        {
            var res = new GroupedResual();
            StepInResual(resual, res, (item, r) =>
            {
                var typeName = item.SourceType.Name;
                if (r.Properties.TryGetValue(typeName, out var list))
                {
                    list.Add(item);
                }
                else
                {
                    r.Properties.Add(typeName, new List<AoAnalizedPropertyItemBase> { item });
                }
                return 1;
            }, (item, r) =>
            {
                var typeName = item.SourceType.Name;
                if (r.Methods.TryGetValue(typeName, out var list))
                {
                    list.Add(item);
                }
                else
                {
                    r.Methods.Add(typeName, new List<AoAnalizedMethodItemBase> { item });
                }
                return 1;
            });
            return res;
        }
        private void StepInResual(AoAnalizedResual resual, GroupedResual gr,
            Func<AoAnalizedPropertyItemBase, GroupedResual, int> propertyAction,
            Func<AoAnalizedMethodItemBase, GroupedResual, int> methodAction)
        {
            foreach (var item in resual.MemberItems)
            {
                if (propertyAction(item, gr) == 0)
                {
                    gr.Properties[GroupedResual.DefaultGroup].Add(item);
                }
            }
            foreach (var item in resual.MethodItems)
            {
                if (methodAction(item, gr) == 0)
                {
                    gr.Methods[GroupedResual.DefaultGroup].Add(item);
                }
            }
            if (resual.Nexts.Length != 0)
            {
                foreach (var item in resual.Nexts)
                {
                    StepInResual(item, gr, propertyAction, methodAction);
                }
            }
        }
    }
}
