

namespace Phyah.Thread
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Concurrent;
    internal class ConcurrentAccessor
    {
        public ConcurrentDictionary<Type, object> Concurrent { get; } = new ConcurrentDictionary<Type, object>();
        public void Add(object value)
        {
            Concurrent.AddOrUpdate(value.GetType(), value, (t, o) => { return value; });
        }
        public object Remove<T>()
        {
            var type = typeof(T);
            object returnValue = default(T);
            if (Concurrent.ContainsKey(type))
            {
                Concurrent.TryRemove(type, out returnValue);
            }
            return returnValue;
        }
        public T Get<T>()
        {
            var type = typeof(T);
            object returnValue = default(T);
            if (Concurrent.ContainsKey(type))
            {
                Concurrent.TryGetValue(type, out returnValue);
            }
            return returnValue is T ? (T)returnValue : default(T);
        }

    }


}
