using Ao.Shared.ForView.Input;
using System;
using System.Collections.Generic;

namespace Ao.Shared.ForView
{
    /// <summary>
    /// 视图创建的上下文
    /// </summary>
    /// <typeparam name="TView">视图类型,一般都说视图基类</typeparam>
    public class ViewBuildContext<TView>
    {
        private Dictionary<string, object> feature;
        /// <summary>
        /// 初始化<see cref="ViewBuildContext{TView}"/>
        /// </summary>
        /// <param name="vm">视图模型</param>
        /// <param name="viewBuilder">视图建造器</param>
        public ViewBuildContext(object vm, IViewBuildable<TView> viewBuilder)
        {
            Vm = vm;
            ViewBuilder = viewBuilder;
        }
        /// <summary>
        /// 视图模型
        /// </summary>
        public object Vm { get; }
        /// <summary>
        /// 表示特性集合
        /// </summary>
        public Dictionary<string,object> Feature
        {
            get
            {
                if (feature==null)
                {
                    feature = new Dictionary<string, object>();
                }
                return feature;
            }
        }
        /// <summary>
        /// 视图建造者
        /// </summary>
        public IViewBuildable<TView> ViewBuilder { get; }
        /// <summary>
        /// <inheritdoc cref="GetString(string)"/>
        /// </summary>
        /// <param name="item">属性项</param>
        /// <param name="default">默认的值</param>
        /// <returns></returns>
        public string GetString(AoAnalizedPropertyItemBase item,string @default)
        {
            var str = GetString(item);
            if (string.IsNullOrEmpty(str))
            {
                return @default;
            }
            return str;
        }
        /// <summary>
        /// <inheritdoc cref="GetString(string, string)"/>
        /// </summary>
        /// <param name="item">属性项</param>
        /// <returns></returns>
        public string GetString(AoAnalizedPropertyItemBase item)
        {
            var attr = item.GetCustomAttribute<StringKeyAttribute>();
            if (attr!=null)
            {
                return GetString(attr.Key);
            }
            string str = GetString($"{item.SourceType.FullName}.{item.ValueName}");
            return str ?? GetString(item.ValueName);
        }
        /// <summary>
        /// 表示从字符串提供者获取一个字符串,
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetString(string key)
        {
            if (ViewBuilder.StringProvider == null || string.IsNullOrEmpty(key))
            {
                return null;
            }
            return ViewBuilder.StringProvider.GetString(key);
        }
        /// <summary>
        /// <inheritdoc cref="GetString(string)"/>
        /// </summary>
        /// <param name="key">资源键</param>
        /// <param name="default">默认值</param>
        /// <returns></returns>
        public string GetString(string key,string @default)
        {
            var str = GetString(key);
            if (string.IsNullOrEmpty(str))
            {
                return @default;
            }
            return str;
        }
    }
}
