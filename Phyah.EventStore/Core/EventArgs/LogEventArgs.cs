using System;
using System.Collections.Generic;
using System.Text;
using Phyah.EventStore.Messages;

namespace Phyah.EventStore
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(Message model, EventContext context) : base(model, context)
        {

        }
    }
}
