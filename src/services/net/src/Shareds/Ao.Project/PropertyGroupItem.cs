namespace Ao.Project
{
    /// <summary>
    /// 表示一个属性组项,如果需要想要服务，就使用<see cref="DIServices.DIService.ServiceProvider"/>
    /// </summary>
    public abstract class PropertyGroupItem : ProjectPart, IPropertyGroupItem
    {
        public static readonly string NodeName = "PropertyGroupItem";
        /// <summary>
        /// 表示装饰工程
        /// </summary>
        public void Decorate()
        {
            if (!Done)
            {
                Done = true;
                OnDecorate();
            }
        }
        protected abstract void OnDecorate();
    }
}
