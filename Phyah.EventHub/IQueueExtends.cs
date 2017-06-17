

namespace Phyah.EventHub
{
    using Phyah.Collection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    public static class IQueueExtends
    {
        public static IEnumerable<T> Enum<T>(this IQueue<T> queue)
        {
            if (queue == null)
            {
                return null;
            }
            var iqclone = queue.Clone();
            var list = new List<T>();
            var it = iqclone.Dequeue();
            do
            {
                list.Add(it);
                it = iqclone.Dequeue();
            }
            while (iqclone.Count > 0);
            return list;
        }
        public static IEnumerable<T> Enum<T>(this IQueue<T> queue,Type type)
        {
            return queue.Enum().Where(m => type.IsAssignableFrom(m.GetType()));
        }
    }
}
