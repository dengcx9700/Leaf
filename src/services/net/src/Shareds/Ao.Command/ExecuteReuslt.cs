namespace Ao.Command
{
    /// <summary>
    /// 执行结果，实现了<see cref="IExecuteReuslt"/>
    /// </summary>
    public class ExecuteReuslt : IExecuteReuslt
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object[] Paramters { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Succeed { get; set; }
    }
}
