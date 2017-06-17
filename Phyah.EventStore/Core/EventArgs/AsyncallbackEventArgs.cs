using System;
using System.Collections.Generic;
using System.Text;
using Phyah.EventStore.Messages;

namespace Phyah.EventStore
{
    public class AsyncallbackEventArgs : EventArgs
    {
        public AsyncallbackEventArgs(Message model, EventContext context) : base(model, context)
        {
        }
    }
}
