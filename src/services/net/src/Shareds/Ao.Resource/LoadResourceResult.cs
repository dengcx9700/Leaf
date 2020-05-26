namespace Ao.Resource
{
    /// <summary>
    /// 表示资源加载的结果
    /// </summary>
    public struct LoadResourceResult
    {
        /// <summary>
        /// 表示被跳过
        /// </summary>
        public static LoadResourceResult SkipResult = new LoadResourceResult(null, true);
        /// <summary>
        /// 表示加载没有成功了
        /// </summary>
        public static LoadResourceResult NotLoadResult = new LoadResourceResult(null, false);


        public LoadResourceResult(IResourceMetadata metadata, bool skip)
        {
            Metadata = metadata;
            Skip = skip;
        }
        /// <summary>
        /// 加载后的资源元数据
        /// </summary>
        public IResourceMetadata Metadata { get; }
        /// <summary>
        /// 加载是否被跳过了
        /// </summary>
        public bool Skip { get; }
        /// <summary>
        /// 是否成功了
        /// </summary>
        public bool Succeed => Metadata != null;
    }
}
