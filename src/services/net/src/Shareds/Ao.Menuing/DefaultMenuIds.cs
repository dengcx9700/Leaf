namespace Pd.Services.Menu
{
    /// <summary>
    /// 预置的菜单id
    /// </summary>
    public static class DefaultMenuIds
    {
        /// <summary>
        /// 新建工程
        /// </summary>
        public static readonly string New = "new";
        /// <summary>
        /// 文件
        /// </summary>
        public static readonly string File = "file";
        /// <summary>
        /// 打开工程
        /// </summary>
        public static readonly string Open = "open";
        /// <summary>
        /// 保存工程
        /// </summary>
        public static readonly string Save = "save";
        /// <summary>
        /// 导出工程
        /// </summary>
        public static readonly string Export = "export";
        /// <summary>
        /// 导入工程
        /// </summary>
        public static readonly string Import = "import";
        /// <summary>
        /// 插件操作
        /// </summary>
        public static readonly string Plug = "plug";
        /// <summary>
        /// 其它(杂项)
        /// </summary>
        public static readonly string Other = "other";
        /// <summary>
        /// 分割符
        /// </summary>
        public static readonly string Separator = "separator";
        /// <summary>
        /// 发现页
        /// </summary>
        public static readonly string Found = "found";
        /// <summary>
        /// 管理页
        /// </summary>
        public static readonly string Manager = "manager";
        /// <summary>
        /// 设置
        /// </summary>
        public static readonly string Setting = "setting";
        /// <summary>
        /// 靠边文档
        /// </summary>
        public static readonly string Avalon = "avalon";
        /// <summary>
        /// 内嵌文档
        /// </summary>
        public static readonly string Document = "document";
        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="parts">路径部分</param>
        /// <returns></returns>
        public static string Combine(params string[] parts)
        {
            if (parts==null)
            {
                return null;
            }
            return string.Join("/", parts);
        }
    }
}
