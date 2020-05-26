namespace Ao.Command
{
    /// <summary>
    /// 对类型<see cref="ICommandManager"/>的扩展
    /// </summary>
    public static class CommandManagerExtensions
    {
        /// <summary>
        /// 添加一个对象命令源
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="obj">目标对象，此对象不能为空</param>
        public static void AddObject(this ICommandManager manager, object obj)
        {
            if (manager is null)
            {
                throw new System.ArgumentNullException(nameof(manager));
            }

            if (obj is null)
            {
                throw new System.ArgumentNullException(nameof(obj));
            }

            manager.Add(new ObjectCommandSource(obj));
        }
        /// <summary>
        /// 新建一个类型，并且加入到命令器中
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="manager"></param>
        public static void AddObject<T>(this ICommandManager manager)
            where T:class,new()
        {
            if (manager is null)
            {
                throw new System.ArgumentNullException(nameof(manager));
            }

            manager.AddObject(new T());
        }

    }
}
