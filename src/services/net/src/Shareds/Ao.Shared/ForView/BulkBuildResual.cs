using System.Collections;
using System.Collections.Generic;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 表示分组后的批量视图结果
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    public class BulkBuildResual<TView> : IEnumerable<BuildResualItem<TView>>
    {
        /// <summary>
        /// 初始化<see cref="BulkBuildResual{TView}"/>
        /// </summary>
        public BulkBuildResual()
        {
            Views = new List<BuildResualItem<TView>>();
            Next = new List<BulkBuildResual<TView>>();
        }
        /// <summary>
        /// 这一层的视图结果
        /// </summary>
        public List<BuildResualItem<TView>> Views { get; }
        /// <summary>
        /// 下一些层
        /// </summary>
        public List<BulkBuildResual<TView>> Next { get; }
        /// <summary>
        /// 全部的视图结果
        /// </summary>
        public IEnumerable<BuildResualItem<TView>> All => GetAll();
        /// <summary>
        /// 获取这一层并且往后的所有视图结果
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BuildResualItem<TView>> GetAll()
        {
            var vs = new List<BuildResualItem<TView>>();
            InsertViews(vs, this);
            return vs;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public IEnumerator<BuildResualItem<TView>> GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void InsertViews(List<BuildResualItem<TView>> views, BulkBuildResual<TView> resual)
        {
            foreach (var item in resual.Views)
            {
                views.Add(item);
            }
            foreach (var item in resual.Next)
            {
                InsertViews(views, item);
            }
        }
    }
}
