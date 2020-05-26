using Ao.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace Pd.Services.Menu
{
    public abstract class MenuServiceBase : PackagingService<DefaultPackage<IMenuMetadata>, IMenuMetadata, DefaultPackage<IMenuMetadata>>, IMenuService
    {

        public MenuServiceBase()
        {
            notfoundMenus = new List<NotFoundMenu>();
        }
        private readonly List<NotFoundMenu> notfoundMenus;
        /// <summary>
        /// <inheritdoc/>,如果path为空就是根节点
        /// </summary>
        /// <param name="path"><inheritdoc/></param>
        /// <param name="actionType"><inheritdoc/></param>
        /// <returns></returns>
        public ActionTestResult CanAction(IMenuNode node, string path, MenuActionTypes actionType)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new ActionTestResult(path, actionType, ActionTestResultTypes.OK)
                {
                    resultNodes = new[] { node }
                };
            }
            var nodes = node.Find(path);
            if (nodes == null || nodes.Length == 0)
            {
                return new ActionTestResult(path, actionType, ActionTestResultTypes.NotFound);
            }
            var okNodes = nodes;
            if (actionType == MenuActionTypes.Replace)
            {
                okNodes = nodes.Where(n => (!n.Metadata.RequestReplace) || n.Metadata.RequestReplace && n.Metadata.ConditionReplace()).ToArray();
            }
            return new ActionTestResult(path, actionType,
                actionType == MenuActionTypes.Replace && okNodes.Length == 0 ? ActionTestResultTypes.RefuseReplace : ActionTestResultTypes.OK)
            {
                resultNodes = okNodes
            };
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="metadata"><inheritdoc/></param>
        /// <returns></returns>
        public bool ValidateMetadata(IMenuMetadata metadata)
        {
            if (metadata == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(metadata.Id))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="type"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Condition(IMenuMetadata type)
        {
            return true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <returns></returns>
        protected override DefaultPackage<IMenuMetadata> MakePackage(Assembly assembly)
        {
            return new DefaultPackage<IMenuMetadata>(assembly);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="views"><inheritdoc/></param>
        /// <returns></returns>
        public override IMenuMetadata[] Add(Assembly assembly, params IMenuMetadata[] views)
        {
            var res = base.Add(assembly, views);

            foreach (var item in views)
            {
                var nodes = SelectMenuNode(item);
                var hasNotNodes = new List<IMenuNode>();
                foreach (var node in nodes)
                {
                    var r = CanAction(node, item.InsertPath, item.ActionType);
                    if (r.ResultType== ActionTestResultTypes.OK)
                    {
                        Insert(item, r);
                        //去插多一次
                        Flush();
                    }
                    else
                    {
                        hasNotNodes.Add(node);
                    }
                }
                if (hasNotNodes.Count != 0)
                {
                    notfoundMenus.Add(new NotFoundMenu(item, hasNotNodes.ToArray()));
                }
            }
            return res;
        }
        protected bool InsertTo(IMenuNode node, IMenuMetadata item)
        {
            var r = CanAction(node, item.InsertPath, item.ActionType);
            return Insert(item, r);
        }
        /// <summary>
        /// 为菜单元数据选择菜单节点
        /// </summary>
        /// <param name="metadata">目标节点</param>
        /// <returns></returns>
        protected abstract IMenuNode[] SelectMenuNode(IMenuMetadata metadata);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Flush()
        {
            if (notfoundMenus.Count > 0)
            {
                var bank = notfoundMenus.ToArray();
                var doneList = new List<NotFoundMenu>();
                foreach (var item in bank)
                {
                    var hasNotNodes = item.NotFoundMenuNodes;
                    foreach (var node in hasNotNodes)
                    {
                        var r = CanAction(node, item.Menu.InsertPath, item.Menu.ActionType);
                        if (r.ResultType== ActionTestResultTypes.OK)
                        {
                            item.DoneNode(node);
                            Insert(item.Menu, r);
                        }
                    }
                    if (item.IsDone)
                    {
                        doneList.Add(item);
                    }
                }
                foreach (var item in doneList)
                {
                    notfoundMenus.Remove(item);
                }
            }
        }
        /// <summary>
        /// 插入菜单节点
        /// </summary>
        /// <param name="item">节点</param>
        /// <param name="r">测试的结果</param>
        /// <returns></returns>
        protected bool Insert(IMenuMetadata item, ActionTestResult r)
        {
            var succeed = false;
            if (string.IsNullOrEmpty(item.Id) && !item.IsSeparator)
            {
                throw new ArgumentException("菜单元数据id不能为null");
            }
            switch (r.ResultType)
            {
                case ActionTestResultTypes.OK:
                    var node = r.ResultNodes.First();
                    switch (item.ActionType)
                    {
                        case MenuActionTypes.After:
                            if (node.Parent == null)
                            {
                                item.InsertFailed(r);//如果是根节点,无法插入
                            }
                            else
                            {
                                node.Parent.Nexts.Add(new MenuNode(item));
                            }
                            break;
                        case MenuActionTypes.Replace:
                            if (!node.IsRoot)//主节点不允许替换
                            {
                                var index = node.Parent.Nexts.IndexOf(node);
                                var parent = node.Parent;
                                parent.Nexts.Remove(node);//断链
                                parent.Nexts.Insert(index, new MenuNode(item));
                            }
                            break;
                        case MenuActionTypes.Before:
                            if (node.Parent == null)
                            {
                                item.InsertFailed(r);//如果是根节点,无法插入
                            }
                            else
                            {
                                node.Parent.Nexts.Add(new MenuNode(item));
                                node.Parent.Nexts.Move(node.Parent.Nexts.Count - 1, 0);//移到最前面
                            }
                            break;
                        case MenuActionTypes.InnerAfter:
                            node.Nexts.Add(new MenuNode(item));
                            break;
                        case MenuActionTypes.InnerBefore:
                            node.Nexts.Add(new MenuNode(item));
                            node.Nexts.Move(node.Nexts.Count - 1, 0);//移动到最前面
                            break;
                        default:
                            throw new NotSupportedException(item.ActionType.ToString());
                    }
                    succeed = true;
                    break;
                case ActionTestResultTypes.NotFound:
                    item.InsertFailed(r);
                    var hasNode = notfoundMenus.Any(m => m.Menu == item);
                    if (!hasNode)
                    {
                        var nodes = SelectMenuNode(item);
                        notfoundMenus.Add(new NotFoundMenu(item, nodes));
                    }
                    break;
                case ActionTestResultTypes.RefuseReplace:
                    item.ReplaceFailed(r);
                    break;
                case ActionTestResultTypes.Unknow:
                    item.InsertFailed(r);
                    break;
                default:
                    break;
            }
            return succeed;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="removedPkg"><inheritdoc/></param>
        /// <returns></returns>
        public override bool Remove(Assembly assembly, DefaultPackage<IMenuMetadata> removedPkg)
        {
            var res = base.Remove(assembly, removedPkg);
            if (res)
            {
                foreach (var item in removedPkg)
                {
                    var nodes = SelectMenuNode(item);
                    if (nodes!=null)
                    {
                        foreach (var node in nodes)
                        {
                            var r = CanAction(node, item.InsertPath, item.ActionType);
                            Remove(item, r);
                        }
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="assembly"><inheritdoc/></param>
        /// <param name="removes"><inheritdoc/></param>
        /// <returns></returns>
        public override IMenuMetadata[] Remove(Assembly assembly, params IMenuMetadata[] removes)
        {
            var res= base.Remove(assembly, removes);
            if (res!=null&&res.Length>0)
            {
                foreach (var remove in removes)
                {
                    var nodes = SelectMenuNode(remove);
                    foreach (var node in nodes)
                    {
                        var r = CanAction(node, remove.InsertPath, remove.ActionType);
                        Remove(remove, r);

                    }
                }
            }
            return res;
        }
        protected void Remove(IMenuMetadata item, ActionTestResult r)
        {
            if (r.ResultType == ActionTestResultTypes.OK)
            {
                switch (item.ActionType)
                {
                    case MenuActionTypes.After:
                    case MenuActionTypes.Replace:
                    case MenuActionTypes.Before:
                        var node = r.ResultNodes.First();
                        var current = node.Nexts.FirstOrDefault(n => n.Metadata == item);
                        if (current != null)//被替换的可能不存在
                        {
                            node.Nexts.Remove(current);
                        }
                        break;
                    case MenuActionTypes.InnerAfter:
                    case MenuActionTypes.InnerBefore:
                        node = r.ResultNodes.First();
                        current = node.Nexts.First(n => n.Metadata == item);
                        node.Nexts.Remove(current);
                        break;
                    default:
                        break;
                }
            }

        }
    }
    public class NotFoundMenu
    {
        private readonly Dictionary<IMenuNode, bool> menuToNodes;
        private int need;
        private readonly object locker = new object();
        public NotFoundMenu(IMenuMetadata menu, params IMenuNode[] nodes)
        {
            if (nodes is null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            Menu = menu ?? throw new ArgumentNullException(nameof(menu));
            menuToNodes = nodes.ToDictionary(n => n, _ => false);
            Nodes = nodes;
        }
        /// <summary>
        /// 需要插入的节点
        /// </summary>
        public IMenuNode[] Nodes { get; }
        /// <summary>
        /// 目标菜单元数据
        /// </summary>
        public IMenuMetadata Menu { get; }
        /// <summary>
        /// 还差几个要插入
        /// </summary>
        public int Need => need;
        /// <summary>
        /// 是否已经就完成了
        /// </summary>
        public bool IsDone => need != 0;
        /// <summary>
        /// 还没插入的节点
        /// </summary>
        public IMenuNode[] NotFoundMenuNodes => menuToNodes.Where(n => !n.Value).Select(n => n.Key).ToArray();
        /// <summary>
        /// 已经完成的节点
        /// </summary>
        public IMenuNode[] DoneMenuNodes => menuToNodes.Where(n => n.Value).Select(n => n.Key).ToArray();
        /// <summary>
        /// 完成节点
        /// </summary>
        /// <param name="node"></param>
        public void DoneNode(IMenuNode node)
        {
            if (menuToNodes.ContainsKey(node) && !menuToNodes[node])
            {
                lock (locker)
                {
                    if (!menuToNodes[node])
                    {
                        menuToNodes[node] = true;
                        need--;
                    }
                }
            }
        }
        /// <summary>
        /// 是否节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsWantNode(IMenuNode node)
        {
            return menuToNodes.ContainsKey(node);
        }
    }
}

