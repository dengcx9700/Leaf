namespace Pd.Services.Menu
{
    /// <summary>
    /// 单菜单服务
    /// </summary>
    public class SingleMenuService : MenuServiceBase
    {
        public SingleMenuService()
        {
            Menu = MenuNode.MakeRoot();
        }

        /// <summary>
        /// 菜单根节点
        /// </summary>
        public IMenuNode Menu { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="metadata"><inheritdoc/></param>
        /// <returns></returns>
        protected override IMenuNode[] SelectMenuNode(IMenuMetadata metadata)
        {
            return new[] { Menu };
        }
    }
}

