using Ao.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Pd.Services.Menu
{
    /// <summary>
    /// 表示对菜单的服务
    /// </summary>
    public interface IMenuService : IPackagingService<DefaultPackage<IMenuMetadata>, IMenuMetadata, DefaultPackage<IMenuMetadata>>
    {
        /// <summary>
        /// 查找是否可以被操作
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="path">目标路径</param>
        /// <param name="actionType">操作类型</param>
        /// <returns></returns>
        ActionTestResult CanAction(IMenuNode node,string path,MenuActionTypes actionType);
        /// <summary>
        /// 验证元数据是否符合，返回元数据是否符合
        /// </summary>
        /// <param name="metadata">目标元数据</param>
        /// <returns></returns>
        bool ValidateMetadata(IMenuMetadata metadata);
        /// <summary>
        /// 刷新节点，让没插进去的菜单查看是否可以插入
        /// </summary>
        void Flush();
    }
}
