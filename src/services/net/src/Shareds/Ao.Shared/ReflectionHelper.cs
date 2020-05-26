using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Ao
{
    /// <summary>
    /// 成员设置器
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="value">目标值</param>
    public delegate void AoMemberSetter<T>(T value);
    /// <summary>
    /// 成员取值器
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <returns>获取到的值</returns>
    public delegate T AoMemberGetter<T>();
    /// <summary>
    /// 固定对象成员取值器
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="inst">获取源</param>
    /// <returns>获取到的值</returns>
    public delegate T AoWithMemberGetter<T>(object inst);
    /// <summary>
    /// 成员调用器
    /// </summary>
    /// <typeparam name="T">目标值</typeparam>
    /// <param name="inst">目标对象</param>
    /// <param name="paramters">参数列表</param>
    /// <returns></returns>
    public delegate T AoMemberInvoker<T>(object inst,params object[] paramters);
    /// <summary>
    /// 成员创建器
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="paramters">参数列表</param>
    /// <returns></returns>
    public delegate T AoMemberNewer<T>(params object[] paramters);
    /// <summary>
    /// 反射帮助
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取一个设置器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="inst">目标对象</param>
        /// <param name="valueType">返回值</param>
        /// <param name="setter">取值方法信息</param>
        /// <returns></returns>
        public static AoMemberSetter<T> GetSetter<T>(object inst, Type valueType, MethodInfo setter)
        {
            var par1 = Expression.Parameter(typeof(object));
            var exp = GenerateChangeTypeExp(par1, par1, valueType);
            var c = Expression.Call(Expression.Constant(inst), setter, Expression.Convert(par1,valueType));
            return Expression.Lambda<AoMemberSetter<T>>(Expression.Block(
                exp, c), false, new[] { par1 }).Compile();
        }
        /// <summary>
        /// 生成更改类型的表达式
        /// </summary>
        /// <param name="arg">目标表达式</param>
        /// <param name="dest">转值的表达式</param>
        /// <param name="targetType">更改到的类型</param>
        /// <returns></returns>
        public static Expression GenerateChangeTypeExp(Expression arg, Expression dest, Type targetType)
        {
            var test = Expression.Equal(
                Expression.Call(arg, typeof(object).GetMethod(nameof(object.GetType))),
                Expression.Constant(targetType));
            Expression f;
            if (targetType.IsValueType)
            {
                f = Expression.Assign(dest, Expression.Call(null,
                typeof(Convert).GetMethod(nameof(Convert.ChangeType),
                new Type[] { typeof(object), typeof(Type) }),
                arg, Expression.Constant(targetType)));
            }
            else
            {
                f = Expression.Assign(dest, Expression.Convert(arg, targetType));
            }
            return Expression.IfThenElse(test, Expression.Empty(), f);
        }
        /// <summary>
        /// 获取一个取值器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="inst">实例表达式</param>
        /// <param name="getter">取值方法</param>
        /// <returns></returns>
        private static AoMemberGetter<T> GetGetter<T>(Expression inst, MethodInfo getter)
        {
            ThrowIfVoid(getter.ReturnType);
            var lab = Expression.Label(typeof(T));

            var c = Expression.Call(inst, getter);
            return Expression.Lambda<AoMemberGetter<T>>(Expression.Label(lab, Expression.TypeAs(c, typeof(T))),
                true).Compile();
        }
        private static void ThrowIfVoid(Type type)
        {
            if (type == typeof(void))
            {
                throw new InvalidOperationException("getter 无法返回值");
            }
        }
        /// <summary>
        /// 获取取值器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="getter">取值方法</param>
        /// <returns></returns>
        public static AoWithMemberGetter<T> GetGetter<T>(MethodInfo getter)
        {
            ThrowIfVoid(getter.ReturnType);
            var par1 = Expression.Parameter(typeof(object));
            var lab = Expression.Label(typeof(T));

            var c = Expression.Call(par1, getter);
            return Expression.Lambda<AoWithMemberGetter<T>>(
                Expression.Label(lab, Expression.TypeAs(c, typeof(T))),
                true,new[] { par1}).Compile();
        }
        /// <summary>
        /// 获取取值器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="inst">目标源</param>
        /// <param name="getter">取值方法</param>
        /// <returns></returns>
        public static AoMemberGetter<T> GetGetter<T>(object inst, MethodInfo getter)
        {
            return GetGetter<T>(Expression.Constant(inst), getter);
        }
        /// <summary>
        /// 获取取值器，从属性名
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="inst">目标对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static AoMemberGetter<T> GetGetter<T>(object inst, string propertyName)
        {
            return GetGetter<T>(inst, inst.GetType().GetProperty(propertyName).GetMethod);
        }
        /// <summary>
        /// 获取设置器，从属性名
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="inst">目标对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static AoMemberSetter<T> GetSetter<T>(object inst, string propertyName)
        {
            var prop = inst.GetType().GetProperty(propertyName);
            return GetSetter<T>(inst, prop.PropertyType, prop.SetMethod);
        }
        /// <summary>
        /// 获取调用器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="paramters">参数信息列表</param>
        /// <param name="returnType">返回值</param>
        /// <param name="caller">调用包装器</param>
        /// <param name="var"></param>
        /// <param name="newer"></param>
        /// <returns></returns>
        public static T GetInvoker<T>(ParameterInfo[] paramters, Type returnType,
            Func<Expression,Expression[], Expression> caller,
            Expression var = null,bool newer=false)
            where T : Delegate
        {
            var pars = new List<ParameterExpression>();
            ParameterExpression inst = null;
            if (!newer&& returnType!=null)
            {
                inst = Expression.Parameter(typeof(object));
                pars.Add(inst);
            }
            var par = Expression.Parameter(typeof(object[]));
            pars.Add(par);
            var ret = Expression.Label(typeof(object));
            var methPars = paramters;
            var valid = Expression.IfThenElse(Expression.Equal(Expression.ArrayLength(par),
                Expression.Constant(methPars.Length)), Expression.Empty(), Expression.Throw(
                    Expression.New(typeof(InvalidOperationException).GetConstructor(new[] { typeof(string) }),
                    Expression.Constant($"参数数量不正确,目的方法需要参数:{methPars.Length}个"))));
            var inPars = new Expression[methPars.Length];
            for (int i = 0; i < methPars.Length; i++)
            {
                inPars[i] = Expression.MakeIndex(par,
                        typeof(string[]).GetProperty("Item"),
                        new[] { Expression.Constant(i) });
                var mp = methPars[i];
                if (!mp.ParameterType.IsGenericParameter&&
                    !mp.ParameterType.IsByRef)
                {
                    inPars[i] = Expression.Convert(inPars[i], methPars[i].ParameterType);
                }
            }
            var c = caller(inst,inPars);
            if (var == null)
            {
                var = Expression.Empty();
            }
            Expression retExp = null;
            if (returnType == typeof(void))
            {
                retExp = Expression.Block(
                    c,
                    Expression.Label(ret, Expression.Constant(null))
                    );
            }
            else
            {
                retExp = Expression.Label(ret, Expression.Convert(c, typeof(object)));
            }
            var block = Expression.Block(
                var,
                valid,retExp);
            if (typeof(T).IsEquivalentTo(typeof(Delegate)))
            {
                return (T)Expression.Lambda(block, false, pars.ToArray()).Compile();
            }
            else
            {
                return Expression.Lambda<T>(block, false, pars.ToArray()).Compile();
            }
        }
        /// <summary>
        /// 获取调用器
        /// </summary>
        /// <param name="inst">目标对象</param>
        /// <param name="method">方法信息</param>
        /// <param name="genericTypes">泛型类型</param>
        /// <returns></returns>
        public static AoMemberInvoker<object> GetInvoker(object inst, MethodInfo method, params Type[] genericTypes)
        {
            return GetInvoker<object>(inst, method, method.ReturnType, genericTypes);
        }
        /// <summary>
        /// 获取调用器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <typeparam name="TReturn">返回值类型</typeparam>
        /// <param name="inst">目标对象</param>
        /// <param name="method">方法信息</param>
        /// <param name="genericTypes">泛型类型</param>
        /// <returns></returns>
        public static AoMemberInvoker<T> GetInvoker<T,TReturn>(object inst, MethodInfo method,params Type[] genericTypes)
        {
            return GetInvoker<T>(inst, method, typeof(TReturn), genericTypes);
        }
        /// <summary>
        /// 获取调用器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <typeparam name="TReturn">返回值类型</typeparam>
        /// <param name="inst">目标对象</param>
        /// <param name="methodName">方法名</param>
        /// <param name="genericTypes">泛型类型</param>
        /// <returns></returns>
        public static AoMemberInvoker<T> GetInvoker<T, TReturn>(object inst, string methodName, params Type[] genericTypes)
        {
            return GetInvoker<T>(inst, inst.GetType().GetMethod(methodName), typeof(TReturn), genericTypes);
        }
        /// <summary>
        /// 获取调用器
        /// </summary>
        /// <param name="inst">目标对象</param>
        /// <param name="method">目标方法</param>
        /// <param name="returnType">返回类型</param>
        /// <param name="genericTypes">泛型类型</param>
        /// <returns></returns>
        public static Delegate GetInvoker(object inst, MethodInfo method, Type returnType, params Type[] genericTypes)
        {
            return GetInvoker<Delegate>(method.GetParameters(), returnType, (arg,ip) =>
            {
                return MakeCall(inst.GetType(), arg, ip, method, genericTypes);
            });
        }
        private static Expression MakeCall(Type instType,Expression inst,Expression[] ip, MethodInfo method, params Type[] genericTypes)
        {
            if (method.IsGenericMethod && genericTypes.Length != 0)
            {
                var argNeed = method.GetGenericArguments().Length;
                if (genericTypes == null || argNeed != genericTypes.Length)
                {
                    throw new InvalidOperationException($"泛型数量与需要的数量不一致，需要{argNeed}个泛型");
                }
                return Expression.Call(Expression.Constant(inst), method.MakeGenericMethod(typeof(object)), ip);
            }
            if (genericTypes.Length != 0)
            {
                throw new ArgumentException("这不是泛型方法，所以不需要传泛型类型进来");
            }
            return Expression.Call(Expression.Convert(inst, instType), method, ip);
        }
        /// <summary>
        /// 获取取值器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="inst">目标对象</param>
        /// <param name="method">目标方法</param>
        /// <param name="returnType">返回类型</param>
        /// <param name="genericTypes">泛型类型</param>
        /// <returns></returns>
        public static AoMemberInvoker<T> GetInvoker<T>(object inst, MethodInfo method, Type returnType, params Type[] genericTypes)
        {
            return GetInvoker<AoMemberInvoker<T>>(method.GetParameters(),returnType, (arg,ip) =>
            {
                return MakeCall(inst.GetType(),arg, ip, method, genericTypes);
            });
        }
        /// <summary>
        /// 获取新建器
        /// </summary>
        /// <param name="targetType">目标类型</param>
        /// <param name="constructorInfo">构造函数</param>
        /// <returns></returns>
        public static AoMemberNewer<object> GetNewer(Type targetType, ConstructorInfo constructorInfo)
        {
            return GetInvoker<AoMemberNewer<object>>(constructorInfo.GetParameters(), targetType, (_,args) =>
            {
                return Expression.New(constructorInfo, args);
            },null,true);
        }
        /// <summary>
        /// 获取新建器
        /// </summary>
        /// <param name="targetType">目标类型</param>
        /// <param name="constructorPars">构造函数参数类型集合</param>
        /// <returns></returns>
        public static AoMemberNewer<object> GetNewer(Type targetType, params Type[] constructorPars)
        {
            return GetNewer(targetType, targetType.GetConstructor(constructorPars));
        }
        /// <summary>
        /// 获取新建器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="constructorInfo">构造函数</param>
        /// <returns></returns>
        public static AoMemberNewer<object> GetNewer<T>(ConstructorInfo constructorInfo)
        {
            return GetNewer(typeof(T), constructorInfo);
        }
        /// <summary>
        /// 获取新建器
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="constructorPars">构造函数参数类型集合</param>
        /// <returns></returns>
        public static AoMemberNewer<object> GetNewer<T>(params Type[] paramterTypes)
        {
            return GetNewer(typeof(T), typeof(T).GetConstructor(paramterTypes));
        }
    }
}
