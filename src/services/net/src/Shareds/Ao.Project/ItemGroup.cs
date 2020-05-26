using System.Threading.Tasks;

namespace Ao.Project
{
    /// <summary>
    /// 表示工程项组
    /// </summary>
    public sealed class ItemGroup : ProjectPartGroup<ItemGroupPart>, IItemGroupPart
    {
        public static readonly string NodeName = "ItemGroup";

        public async Task ConductAsync()
        {
            Done = true;
            foreach (var item in Items)
            {
                await item.ConductAsync();
            }
        }
    }
}
