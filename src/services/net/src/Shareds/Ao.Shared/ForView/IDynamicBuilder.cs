using System.Collections;
using System.Collections.ObjectModel;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示一个动态视图器
    /// </summary>
    /// <typeparam name="TView">建造的视图类型</typeparam>
    public interface IDynamicBuilder<TView>
    {
        /// <summary>
        /// 当前引用的分析器
        /// </summary>
        IAoAnalizer Analizer { get; }
        /// <summary>
        /// 当前引用的视图建造器
        /// </summary>
        IViewBuildable<TView> ViewBuilders { get; }
        /// <summary>
        /// 当前的视图集合
        /// </summary>
        ObservableCollection<TView> Views { get; }
        /// <summary>
        /// 在当前情况与上一个对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="deep">是否深入解析,与<seealso cref="AoAnalizer.Analize(object, bool, AnalizingOptions)"/>的deep一样</param>
        /// <param name="options">分析的可选项</param>
        /// <param name="condition">条件</param>
        void With(object obj, bool deep = true, AnalizingOptions options = null, System.Predicate<AoAnalizedPropertyItemBase> condition=null);
        /// <summary>
        /// 与上一个视图
        /// </summary>
        /// <param name="view">目标视图</param>
        void With(TView view);
        /// <summary>
        /// 当前的对象生成一个列表
        /// </summary>
        /// <returns></returns>
        IList ToList();
        /// <summary>
        /// 重置
        /// </summary>
        void Reset();
        /// <summary>
        /// 加载一个对象列表，并且与上他们
        /// </summary>
        /// <param name="list">目标列表</param>
        /// <param name="deep">是否深入解析</param>
        void FromList(IList list, bool deep = true);
    }
}