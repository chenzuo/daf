using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAF.Core
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection == null)
                return;
            foreach (T ele in collection)
                action(ele);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            if (collection == null)
                return;
            int idx = 0;
            foreach (T ele in collection)
                action(ele, idx++);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Func<T, bool> action)
        {
            if (collection == null)
                return;
            foreach (T ele in collection)
            {
                bool result = action(ele);
                if (!result)
                    return;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Func<T, int, bool> action)
        {
            if (collection == null)
                return;
            int idx = 0;
            foreach (T ele in collection)
            {
                bool result = action(ele, idx++);
                if (!result)
                    return;
            }
        }

        public static R ForEach<T, R>(this IEnumerable<T> collection, Func<T, R> action)
        {
            return ForEach<T, R>(collection, action, o => o != null);
        }

        public static R ForEach<T, R>(this IEnumerable<T> collection, Func<T, R> action, Predicate<R> breakNextProvicerAction)
        {
            if (collection == null)
                return default(R);
            foreach (T ele in collection)
            {
                R result = action(ele);
                if (breakNextProvicerAction(result))
                    return result;
            }
            return default(R);
        }

        public static int ElementIndex<T>(this IEnumerable<T> collection, T obj)
        {
            if (collection == null)
                return -1;
            int idx = -1;
            foreach (T ele in collection)
            {
                idx++;
                if (ele.Equals(obj))
                    return idx;
            }
            return -1;
        }

        public static int ElementIndex<T>(this IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (collection == null)
                return -1;
            int idx = -1;
            foreach (T ele in collection)
            {
                idx++;
                if (predicate(ele))
                    return idx;
            }
            return -1;
        }

        public static IEnumerable<U> Cast<T, U>(this IEnumerable<T> collection, Func<T, U> converter)
        {
            if (collection == null)
                return Enumerable.Empty<U>();
            List<U> list = new List<U>();
            foreach (T obj in collection)
                list.Add(converter(obj));
            return list;
        }

        public static IList<T> AsList<T>(this IEnumerable<T> collection)
        {
            List<T> list = new List<T>();
            if (collection != null)
                list.AddRange(collection);
            return list;
        }

        public static void Merge<T>(this IList<T> collection, IEnumerable<T> targets, Func<IEnumerable<T>, T, bool> exists)
        {
            if (collection == null)
                return;
            foreach (T obj in targets)
                if (exists(collection, obj) == false)
                    collection.Add(obj);
        }
    }
}
