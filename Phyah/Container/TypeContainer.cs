

namespace Phyah.Container
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.Concurrent;
    public class TypeContainer
    {
        private static TypeContainer _Container;
        private static object locker = new object();

        protected ConcurrentDictionary<Type, Type> Dictionary;
        protected TypeContainer() { Dictionary = new ConcurrentDictionary<Type, Type>(); }
        public static TypeContainer Container
        {
            get
            {
                if (_Container == null)
                {
                    lock (locker)
                    {
                        if (_Container == null) { _Container = new TypeContainer(); }

                    }
                }
                return _Container;
            }
        }

        public void Inject(Type interType, Type impType)
        {
            Dictionary.AddOrUpdate(interType, impType, (inter, imp) => { return imp; });
        }
        public void Inject<T>(Type impType) => Inject(typeof(T), impType);
        public void Inject<baseType, ImpType>() => Inject(typeof(baseType), typeof(ImpType));
        public Type Fetch(Type type)
        {
            Type fetchType = default(Type);
            if(Dictionary.TryGetValue(type,out fetchType))
            {
                return fetchType;
            }
            return null;
        }
        public Type Fetch<T>()
        {
            Type fetchType = default(Type);
            Type baseType = typeof(T);
            if (Dictionary.TryGetValue(baseType, out fetchType))
            {
                return fetchType;
            }
            return fetchType;
        }
    }
}
