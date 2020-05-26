using System.Threading.Tasks;

namespace Ao.Project
{
    /// <summary>
    /// 表示一个组部分
    /// </summary>
    public abstract class ItemGroupPart : ProjectPart, IItemGroupPart
    {
        public static readonly string NodeName = "ItemGroupPart";
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public async Task ConductAsync()
        {
            if (!Done)
            {
                Done = true;
                await OnConductAsync();
            }
        }

        protected abstract Task OnConductAsync();
    }
}
