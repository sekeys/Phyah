using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Phyah.Thread
{
    public class Accessor<T>
    {
        static AsyncLocal<ConcurrentAccessor> _asyncLocalContext;
        static object locker = new object();
        internal static AsyncLocal<ConcurrentAccessor> Context
        {
            get
            {
                if (_asyncLocalContext == null)
                {
                    lock (locker)
                    {
                        if (_asyncLocalContext == null)
                        {
                            _asyncLocalContext = new AsyncLocal<ConcurrentAccessor>();
                            _asyncLocalContext.Value = new ConcurrentAccessor();
                        }
                    }
                }
                if (_asyncLocalContext.Value == null)
                {
                    lock (locker)
                    {
                        if (_asyncLocalContext.Value == null)
                        {
                            _asyncLocalContext.Value = new ConcurrentAccessor();
                        }
                    }
                }
                return _asyncLocalContext;
            }
        }
        
        public static T Current
        {
            get
            {
                var type = typeof(T);
                var returnObject = default(T);
                if (_asyncLocalContext.Value.Concurrent.ContainsKey(type))
                {
                    return _asyncLocalContext.Value.Get<T>();
                }
                return returnObject;
            }
            set
            {
                Context.Value.Add(value);
            }
        }
    }
}
