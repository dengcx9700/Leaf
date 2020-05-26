namespace Ao.Project
{
    /// <summary>
    /// 表示属性组
    /// </summary>
    public sealed class PropertyGroup : ProjectPartGroup<PropertyGroupItem>, IPropertyGroupItem
    {
        public static readonly string NodeName = "PropertyGroup";
        public void Decorate()
        {
            Done = true;
            foreach (var item in Items)
            {
                item.Decorate();
            }
        }
    }
}
