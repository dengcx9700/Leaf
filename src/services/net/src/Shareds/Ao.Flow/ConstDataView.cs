namespace Ao.Flow
{
    /// <summary>
    /// 固定值的强类型数据视图
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConstDataView<T> : IDataView<T>, IDataView
    {
        private readonly T value;

        public ConstDataView(T value)
        {
            this.value = value;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public T Value => value;

        object IDataView.Value => value;
    }
    /// <summary>
    /// 固定值的数据视图
    /// </summary>
    public class ConstDataView : ConstDataView<object>
    {
        public ConstDataView(object value) : base(value)
        {
        }
    }
}
