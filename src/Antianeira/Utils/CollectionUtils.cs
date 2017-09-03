using System.Collections.Generic;

namespace Antianeira.Utils
{
    public static class CollectionUtils
    {
        public static Queue<T> ToQueue<T>(this IEnumerable<T> items)
        {
            return new Queue<T>(items);
        }

        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
        }

        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue @default = default(TValue))
        {
            if (dic.TryGetValue(key, out TValue value))
            {
                return value;
            }
            return @default;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}
