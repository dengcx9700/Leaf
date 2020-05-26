using Ao.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ao
{
    /// <summary>
    /// 表示一个属性分析器
    /// </summary>
    public class AoAnalizer : IAoAnalizer
    {
        /// <summary>
        /// 默认的属性分析器，分析设置为默认
        /// </summary>
        public static readonly AoAnalizer Default = new AoAnalizer();
        /// <summary>
        /// 初始化<see cref="AoAnalizer"/>
        /// </summary>
        /// <param name="settings"></param>
        public AoAnalizer(AoAnalizeSettings settings)
        {
            Settings = settings;
        }
        /// <summary>
        /// 初始化<see cref="AoAnalizer"/>
        /// </summary>
        public AoAnalizer()
        {
            Settings = new AoAnalizeSettings();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public AoAnalizeSettings Settings { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="inst"><inheritdoc/></param>
        /// <param name="deep"><inheritdoc/></param>
        /// <param name="options"><inheritdoc/></param>
        /// <returns></returns>
        public AoAnalizedResual Analize(object inst, bool deep = true, AnalizingOptions options=null)
        {
            if (inst == null)
            {
                throw new ArgumentNullException(nameof(inst));
            }
            var t = inst.GetType();
            if (!t.IsClass)
            {
                throw new InvalidOperationException($"类型{t.FullName}不是类，无法解析");
            }
            //寻找可解析的值
            return DeepAnalize(new AoAnalizeContext(inst) { AnalizedObject = new HashSet<object> { inst } }, deep,options);
        }
        /// <summary>
        /// 深入解析
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <param name="deep">是否深入</param>
        /// <param name="options">解析可选项</param>
        /// <returns></returns>
        protected virtual AoAnalizedResual DeepAnalize(AoAnalizeContext context, bool deep, AnalizingOptions options = null)
        {
            if (context.Source != null && !context.SourceType.IsValueType)
            {
                var res = AnalizePart(context,options);
                var step = res.MemberItems.Where(m => !m.ValueType.IsValueType).ToArray();
                var nexts = new List<AoAnalizedResual>(step.Length);
                for (int i = 0; i < step.Length; i++)
                {
                    var stepItem = step[i];
                    if (stepItem.CanGet)
                    {
                        var val = stepItem.Getter();
                        if (val != null && deep && CanStepIn(context, stepItem))
                        {
                            context.AnalizedObject.Add(val);
                            var dres = DeepAnalize(new AoAnalizeContext(val)
                            {
                                AnalizedObject = context.AnalizedObject
                            }, deep,options);
                            if (dres != null)
                            {
                                nexts.Add(dres);
                            }
                        }
                    }

                }
                res.Nexts = nexts.ToArray();
                return res;
            }
            return null;
        }
        /// <summary>
        /// 返回一个值，指示当前环境是否可用踏进
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <param name="propertyItem">属性项</param>
        /// <returns></returns>
        protected virtual bool CanStepIn(AoAnalizeContext context, AoAnalizedPropertyItemBase propertyItem)
        {
            
            var sta = propertyItem.GetCustomAttribute<AoNotStepInAttribute>();
            if ((sta != null) && (!sta.CanStepIn(context, propertyItem)))
            {
                return false;
            }
            var propVal = propertyItem.Getter();
            if (Settings.NotStepInArray && propVal is IEnumerable)
            {
                return false;
            }
            if (context.Source == propVal && Settings.IgnoreSelfLoop)//自循环
            {
                return false;
            }
            if (context.AnalizedObject.Contains(propVal) && Settings.IgnoreMutualReference)//相互引用
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 解析部分
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <param name="options">解析可选项</param>
        /// <returns></returns>
        protected virtual AoAnalizedResual AnalizePart(AoAnalizeContext context, AnalizingOptions options)
        {
            //深入解析
            var mbers = GenerateAnalizedPropertyItems(context,options);
            var invos = GenerateAnalizedMethodItems(context);
            return new AoAnalizedResual(context.Source)
            {
                MemberItems = mbers,
                MethodItems = invos
            };
        }
        /// <summary>
        /// 生成解析属性项集合
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <param name="options">解析可选项</param>
        /// <returns></returns>
        protected virtual AoAnalizedPropertyItemBase[] GenerateAnalizedPropertyItems(AoAnalizeContext context, AnalizingOptions options)
        {
            var props = GetAndFilterProperties(context);
            var mbers = new List<AoAnalizedPropertyItemBase>(props.Length);
            for (int i = 0; i < props.Length; i++)
            {
                var propitem = GeneratePropertyItem(context, props[i]);
                if (options?.SkipConditions != null)
                {
                    var needSkip = false;
                    for (int j = 0; j < options.SkipConditions.Length; j++)
                    {
                        if (options.SkipConditions[j].Condition(this, propitem))
                        {
                            needSkip = true;
                            break;
                        }
                    }
                    if (needSkip)
                    {
                        continue;
                    }
                }
                mbers.Add(propitem);
            }
            return mbers.ToArray();
        }
        /// <summary>
        /// 生成解析方法项集合
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <returns></returns>
        protected virtual AoAnalizedMethodItemBase[] GenerateAnalizedMethodItems(AoAnalizeContext context)
        {
            var meths = GetAndFilterMethods(context);
            var invos = new AoAnalizedMethodItemBase[meths.Length];
            for (int i = 0; i < meths.Length; i++)
            {
                invos[i] = GenerateMethodItem(context, meths[i]);
            }
            return invos;
        }
        /// <summary>
        /// 获取并筛选方法集合
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <returns></returns>
        protected virtual MethodInfo[] GetAndFilterMethods(AoAnalizeContext context)
        {
            return context.SourceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(m => MethodCanAnalize(m) && !m.IsGenericMethod && !m.IsGenericMethodDefinition && !m.IsSpecialName).ToArray();
        }
        /// <summary>
        /// 获取并筛选属性
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <returns></returns>
        protected virtual PropertyInfo[] GetAndFilterProperties(AoAnalizeContext context)
        {
            return context.SourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(m => PropertyCanAnalize(m) && !m.IsSpecialName).ToArray();
        }
        /// <summary>
        /// 生成属性项
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <param name="prop">属性信息</param>
        /// <returns></returns>
        protected virtual AoAnalizedPropertyItemBase GeneratePropertyItem(AoAnalizeContext context, PropertyInfo prop)
        {
            return new AoAnalizedPropertyItem(context.Source, prop);
        }
        /// <summary>
        /// 生成方法项
        /// </summary>
        /// <param name="context">解析上下文</param>
        /// <param name="method">方法信息</param>
        /// <returns></returns>
        protected virtual AoAnalizedMethodItemBase GenerateMethodItem(AoAnalizeContext context, MethodInfo method)
        {
            return new AoAnalizedMethodItem(context.Source, method);
        }
        /// <summary>
        /// 返回一个值，指示当前方法是否可以被解析
        /// </summary>
        /// <param name="m">方法信息</param>
        /// <returns></returns>
        protected virtual bool MethodCanAnalize(MethodInfo m)
        {
            var res = !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") &&
                m.GetCustomAttribute<AoIgnoreAttribute>() == null &&
                !m.IsGenericMethod &&
                !m.IsGenericMethodDefinition;
            if (res && Settings.IgnoreWithoutUnit)
            {
                return m.GetCustomAttribute<AoUnitAttribute>() != null;
            }
            return res;
        }
        /// <summary>
        /// 返回一个值，指示属性是否可以被解析
        /// </summary>
        /// <param name="p">属性信息</param>
        /// <returns></returns>
        protected virtual bool PropertyCanAnalize(PropertyInfo p)
        {
            //var res = !p.PropertyType.IsGenericType &&
            //    p.GetCustomAttribute<AoIgnoreAttribute>() == null &&
            //    (p.PropertyType.IsValueType || !(p.PropertyType.BaseType == typeof(Array)) ||
            //    p.PropertyType.GetInterface(nameof(IEnumerable)) == null);
            var res = p.GetCustomAttribute<AoIgnoreAttribute>() == null;
            if (res && Settings.IgnoreWithoutUnit)
            {
                return p.GetCustomAttribute<AoUnitAttribute>() != null;
            }
            return res;
        }
    }
}
