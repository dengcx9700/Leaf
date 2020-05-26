using System;
using System.Reflection;

namespace Ao.DI.Lookup
{
    public interface IServiceCreator
    {
        /// <summary>
        /// 选择合适的构造函数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="servicesInfo"></param>
        /// <returns></returns>
        ConstructorInfo SelectConstructor(Type target, ServicesInfo servicesInfo);
        /// <summary>
        /// 创建创建者
        /// </summary>
        /// <param name="targetType">实现对象</param>
        /// <param name="createNewInfo"></param>
        /// <returns></returns>
        ServiceNewer CreateNewer(Type targetType, CreateNewInfo createNewInfo);
    }
}
