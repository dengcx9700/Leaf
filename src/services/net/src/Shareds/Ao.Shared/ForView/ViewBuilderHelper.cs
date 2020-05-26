using System.Collections.Generic;
using System.Reflection;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 视图建造器的帮助<see cref="ForViewBuilder{TView}"/>
    /// </summary>
    public static class ViewBuilderHelper
    {
        /// <summary>
        /// 从程序集寻找视图建造器
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<IViewBuilder<TView>> Find<TView>(Assembly assembly)
        {
            var target = typeof(IViewBuilder<TView>);
            var types = assembly.GetTypes();
            foreach (var t in types)
            {
                if (t.GetInterface(target.Name)!=null)
                {
                    var newer = ReflectionHelper.GetNewer(t);
                    yield return (IViewBuilder<TView>)newer();
                }
            }
        }
        /// <summary>
        /// 调用程序集，<inheritdoc cref="Find{TView}(Assembly)"/>
        /// </summary>
        /// <typeparam name="TView">视图类型</typeparam>
        /// <returns></returns>
        public static IEnumerable<IViewBuilder<TView>> FindFromExecuting<TView>()
        {
            return Find<TView>(Assembly.GetExecutingAssembly());
        }
    }
}
