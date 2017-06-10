

namespace Phyah.EventHub
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class PriorityReceptorStore : IReceptorStore
    {
        public ConcurrentDictionary<string, IEnumerable<IReceptor>> Dictionary;
        public PriorityReceptorStore()
        {
        }

        public IEnumerable<IReceptor> Match(string name)
        {
            IEnumerable<IReceptor> receps = null;
            if (Dictionary.ContainsKey(name))
            {
                Dictionary.TryGetValue(name, out receps);
            }
            return receps;
        }
        public IEnumerable<IReceptor> Match(string name, Type type)
        {
            IEnumerable<IReceptor> receps = null;
            if (Dictionary.ContainsKey(name))
            {
                Dictionary.TryGetValue(name, out receps);
            }
            return receps.Where(m => type.IsAssignableFrom(m.GetType()));
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
                iqueue.Append(receptor);
            }
            else
            {
                IEnumerable<IReceptor> queue = new List<IReceptor>();
                queue.Append(receptor);
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
        public void Unstore(string name)
        {
            IEnumerable<IReceptor> receps;
            if (Dictionary.ContainsKey(name))
            {
                Dictionary.TryRemove(name, out receps);
            }
        }
        public void Unstore(string name, IReceptor receptor)
        {
            IEnumerable<IReceptor> receps;
            if (Dictionary.ContainsKey(name))
            {
                receps = Dictionary[name];
                receps = receps.Where(m => !m.Equals(receptor));
                Dictionary.AddOrUpdate(name, receps, (key, pre) => {
                    return receps;
                });
            }
        }
    }
}
