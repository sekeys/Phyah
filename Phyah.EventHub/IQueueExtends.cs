

namespace Phyah.EventHub
{
    using Phyah.Collection;
    using System;
    using System.Collections.Generic;
    public static class IQueueExtends
    {
        public static IEnumerable<T> Enum<T>(this IQueue<T> queue)
        {
            if (queue == null)
            {
                return null;
            }
            //var clone=queue.
        }
        public static IEnumerable<T> Enum<T>(this IQueue<T> queue,Type type)
        {
            if (queue == null)
            {
                return null;
            }
            //var clone=queue.
        }
    }
}
