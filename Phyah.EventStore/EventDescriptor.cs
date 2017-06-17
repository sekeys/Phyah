using Phyah.EventStore.Attributes;
using Phyah.EventStore.Enumerate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Phyah.EventStore
{
    public class EventDescriptor
    {
        private IEventHandler _Handler;
        public IList<IFilter> Filters { get; private set; } = new List<IFilter>();
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName
        {
            get;
            set;
        }
        /// <summary>
        /// 事件处理
        /// </summary>
        public IEventHandler Handler
        {
            get;
            set;
        }

        /// <summary>
        /// 是否异步
        /// </summary>
        public SynchronizeTypes Asynchronous
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// 事件请求时间限制
        /// </summary>
        public int Timeout
        {
            get;
            set;
        }

        public void Match()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 栓选
        /// </summary>
        public void Filter(EventArgs args, EventContext context)
        {
            throw new System.NotImplementedException();
        }

        public static EventDescriptor Descriptions(string name,IEventHandler handler)
        {
            var descriptor = new EventDescriptor();
            descriptor.Handler = handler;
            descriptor.EventName = name;
            var descriptType = descriptor.GetType();
            var typeInfo = handler.GetType().GetTypeInfo();
            foreach (var item in typeInfo.GetCustomAttributes<PropertyAttribute>())
            {
                var pinfo = descriptType.GetProperty(item.Property, BindingFlags.Public);
                pinfo.SetValue(descriptor, item.Value);
            }

            foreach (var item in typeInfo.GetCustomAttributes<FilterAttribute>())
            {
                descriptor.Filters.Add(item.Filter);
            }

            foreach (var item in typeInfo.GetCustomAttributes<AuthenticationAttribute>())
            {
                descriptor.Filters.Add(new AuthenticationFilter(item.Authentication));
            }

            var asynch = typeInfo.GetCustomAttribute<SynchronizeAttribute>();
            if (asynch != null)
            {
                descriptor.Asynchronous = asynch.Synchronize;
            }
            return descriptor;
        }
        internal class AuthenticationFilter : IFilter
        {
            IAuthentication Authentication;
            public AuthenticationFilter(IAuthentication authentication)
            {
                Authentication = authentication;
            }
            public bool Filter(EventContext context)
            {
                return Authentication.Authenticate(context);
            }
        }
    }
}