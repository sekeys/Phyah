using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public class IEventStore
    {
        readonly ConcurrentDictionary<string, IEnumerable<EventDescriptor>> Stores;

        /// <param name="name">事件</param>
        public IEnumerate<IEventHandler> Match(string name)
        {
            throw new System.NotImplementedException();
        }

        /// <param name="name">事件名称</param>
        /// <param name="descriptor">事件描述</param>
        public void Store(string name, EventDescriptor descriptor)
        {
            throw new System.NotImplementedException();
        }

        public void Unstore(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}