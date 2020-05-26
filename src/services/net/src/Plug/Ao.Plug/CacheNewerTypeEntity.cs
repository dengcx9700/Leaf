using System;
using System.Reflection;

namespace Ao.Plug
{
    /// <summary>
    /// 缓存新建器的类型实体
    /// </summary>
    public class CacheNewerTypeEntity : ITypeEntity
    {
        public CacheNewerTypeEntity(Type targetType, ConstructorInfo constructor)
        {
            TargetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            Constructor = constructor;
            newer = new Lazy<AoMemberNewer<object>>(BuildNewer, true);
        }

        public CacheNewerTypeEntity(Type targetType)
            :this(targetType,null)
        {
        }

        private readonly Lazy<AoMemberNewer<object>> newer;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Type TargetType { get; }
        /// <summary>
        /// 生成器
        /// </summary>
        public AoMemberNewer<object> Newer => newer.Value;
        /// <summary>
        /// 构造器
        /// </summary>
        public ConstructorInfo Constructor { get; }

        private AoMemberNewer<object> BuildNewer()
        {
            if (Constructor==null)
            {
                return ReflectionHelper.GetNewer(TargetType);
            }
            return ReflectionHelper.GetNewer(TargetType, Constructor);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="params"><inheritdoc/></param>
        /// <returns></returns>
        public object Make(params object[] @params)
        {
            return Newer(@params);
        }
    }
}
