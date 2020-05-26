using System;
using System.Collections.Generic;

namespace Pd.Services.Menu
{
    /// <summary>
    /// 操作查询结果
    /// </summary>
    public class ActionTestResult
    {
        public ActionTestResult(string path, MenuActionTypes actionType, ActionTestResultTypes resultType)
        {
            Path = path;
            ActionType = actionType;
            ResultType = resultType;
        }
        internal IMenuNode[] resultNodes;
        /// <summary>
        /// 目标路径
        /// </summary>
        public string Path { get; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public MenuActionTypes ActionType { get;  }
        /// <summary>
        /// 返回结果
        /// </summary>
        public ActionTestResultTypes ResultType { get; }
        /// <summary>
        /// 成功的的节点数组
        /// </summary>
        public IMenuNode[] ResultNodes => resultNodes ??
#if NET452
            new IMenuNode[0]
#else
            Array.Empty<IMenuNode>()
#endif
            ;
    }
}
