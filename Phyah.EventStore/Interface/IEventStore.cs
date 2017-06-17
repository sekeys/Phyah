using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Phyah.EventStore
{
    public abstract class EventStore
    {
        readonly ConcurrentDictionary<string, IList<EventDescriptor>> Stores;

        /// <param name="name">事件</param>
        public virtual EventExecutor Match(string name)
        {
            EventExecutor exe = new EventExecutor();
        }

        /// <param name="name">事件名称</param>
        /// <param name="descriptor">事件描述</param>
        public virtual void Store(string name, EventDescriptor descriptor)
        {
            if (Stores.ContainsKey(name))
            {
                Stores[name].Add(descriptor);
            }
        }

        public virtual void Unstore(string name, EventDescriptor descriptor)
        {
            if (Stores.ContainsKey(name))
            {
                Stores[name].Remove(descriptor);
            }
        }

        public virtual void Unregister(string name)
        {
            IList< EventDescriptor> value = null;
            if (Stores.ContainsKey(name))
            {
                Stores.TryRemove(name, out value);
            }
        }

        public virtual void Register(string name)
        {
            if (Stores.ContainsKey(name))
            {
                return;
            }
            Stores.TryAdd(name, new List<EventDescriptor>());
        }
    }
}