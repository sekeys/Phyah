using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Phyah.EventStore.Attributes;

namespace Phyah.EventStore.Core
{
    public class DefaultEventArgsFactory : EventArgsFactory
    {
        public override EventArgs Create(Messages.Message message,EventContext context)
        {
            var eventArgsAttr = message.GetType().GetTypeInfo().GetCustomAttribute<EventArgsAttribute>();
            if (eventArgsAttr != null)
            {
                return eventArgsAttr.Create();
            }
            else
            {
                
                return new SimpleEventArgs(message,context);
            }

        }
    }
}
