

namespace Phyah.EventHub
{
    using Phyah.Collection;
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Linq;

    public class DefaultReceptorStore : IReceptorStore
    {
        public ConcurrentDictionary<string, IQueue<IReceptor>> Dictionary = new ConcurrentDictionary<string, IQueue<IReceptor>>();

        public IEnumerable<IReceptor> Match(string name)
        {
            IQueue<IReceptor> receps = null;
            if (Dictionary.ContainsKey(name))
            {
                Dictionary.TryGetValue(name, out receps);
            }
            return receps.Enum();
        }
        public IEnumerable<IReceptor> Match(string name,Type type)
        {
            IQueue<IReceptor> receps = null;
            if (Dictionary.ContainsKey(name))
            {
                Dictionary.TryGetValue(name, out receps);
            }
            return receps.Enum(type);
        }

        public IEnumerable<IReceptor> Match<T>() where T : IReceptor
        {
            var name = nameof(T);
            if (name.IndexOf("Receptor") > 0)
            {
                name = name.Substring(0, name.Length - 8);
            }
            return Match(name, typeof(T));
        }

        public void Store(string name, IReceptor receptor)
        {
            if (Dictionary.ContainsKey(name))
            {
                var iqueue = Dictionary[name];
                iqueue.Enqueue(receptor);
            }
            else
            {
                IQueue<IReceptor> queue = new StoreQueue<IReceptor>();
                queue.Enqueue(receptor);
                Dictionary.TryAdd(name, queue);
            }
        }

        public void Store(IReceptor receptor)
        {
            var name = receptor.GetType().Name;
            if (name.IndexOf("Receptor") > 0)
            {
                name = name.Substring(0, name.Length - 8);
            }
            Store(name, receptor);
        }
    }
}
