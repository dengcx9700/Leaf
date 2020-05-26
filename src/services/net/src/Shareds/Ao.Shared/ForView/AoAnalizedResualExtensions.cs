using Ao.Shared.ForView;
using System;
using System.Text;

namespace Ao
{
    /// <summary>
    /// 对<see cref="AoAnalizedResual"/>的扩展
    /// </summary>
    public static class AoAnalizedResualExtensions
    {
        /// <summary>
        /// 从解析后的结果批量创建视图
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="resual"></param>
        /// <param name="vm">上下文模型</param>
        /// <param name="builder">创建器</param>
        /// <returns></returns>
        public static BulkBuildResual<TView> Build<TView>(this AoAnalizedResual resual, object vm, IViewBuildable<TView> builder)
        {
            var res = new BulkBuildResual<TView>();
            StepBuild(res, vm, resual, builder);
            return res;
        }
        private static void StepBuild<TView>(BulkBuildResual<TView> bkres, object vm, AoAnalizedResual resual, IViewBuildable<TView> builder)
        {
            foreach (var item in resual.MemberItems)
            {
                var v = builder.Build(vm, item);
                if (v != null)
                {
                    bkres.Views.Add(new BuildResualItem<TView>(v, item));
                }
            }
            if (resual.Nexts != null && resual.Nexts.Length > 0)
            {
                foreach (var item in resual.Nexts)
                {
                    var r = new BulkBuildResual<TView>();
                    StepBuild(r, vm, item, builder);
                    bkres.Next.Add(r);
                }
            }
        }

    }
}
