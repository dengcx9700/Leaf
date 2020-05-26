using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Ao.Core.Lru
{
    /// <summary>
    /// lru缓存器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class LruCacher<TKey,TValue>
    {
        private readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> caches;
        private readonly LinkedList<KeyValuePair<TKey,TValue>> linkedList;
        private readonly object locker;
        /// <summary>
        /// 最大个数
        /// </summary>
        public int Max { get; }
        /// <summary>
        /// 当前的个数
        /// </summary>
        public int Count => linkedList.Count;

        public LruCacher(int max)
        {
            if (max<=0)
            {
                throw new RankException("最大值只能大于0");
            }
            locker = new object();
            Max = max;
            caches = new Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>>();
            linkedList = new LinkedList<KeyValuePair<TKey, TValue>>();
        }
        /// <summary>
        /// 获取某一项，如跟失败返回默认值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            if (caches.TryGetValue(key,out var value))
            {
                lock (locker)
                {
                    linkedList.Remove(value);
                    linkedList.AddLast(value);
                }
                return value.Value.Value;
            }
            return default(TValue);
        }
        /// <summary>
        /// 添加一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key,TValue value)
        {
            lock (locker)
            {
                if (caches.ContainsKey(key))
                {
                    var cacheEntity=caches[key];
                    linkedList.Remove(cacheEntity);
                    linkedList.AddLast(cacheEntity);
                    return;
                }
                if (linkedList.Count >= Max)
                {
                    var fs = linkedList.First;
                    linkedList.RemoveFirst();
                    caches.Remove(fs.Value.Key);
                }
                var val = new LinkedListNode<KeyValuePair<TKey, TValue>>(new KeyValuePair<TKey, TValue>(key, value));
                caches.Add(key,val);
                linkedList.AddLast(val);
            }
        }
    }
}
