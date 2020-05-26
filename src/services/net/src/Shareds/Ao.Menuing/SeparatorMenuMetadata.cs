namespace Pd.Services.Menu
{
    /// <summary>
    /// 表示分隔符菜单
    /// </summary>
    public sealed class SeparatorMenuMetadata : MenuMetadataBase
    {
        public SeparatorMenuMetadata(string path=null)
        {
            IsSeparator = true;
            InsertPath = path;
        }
    }
}
