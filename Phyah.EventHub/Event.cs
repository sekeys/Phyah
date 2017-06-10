using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Collection;

namespace Phyah.EventHub
{
    public class Event : IEvent
    {
        readonly object sender;
        public object Sender => sender;
        readonly object[] arguments;
        public object[] Arguments => arguments;

        public Event(object sender,object[] arguments)
        {
            this.sender = sender;
            this.arguments = arguments;
        }
    }
}
