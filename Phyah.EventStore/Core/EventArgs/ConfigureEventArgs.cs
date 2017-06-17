using System;
using System.Collections.Generic;
using System.Text;
using Phyah.EventStore.Messages;

namespace Phyah.EventStore
{
    public class ConfigureEventArgs : EventArgs
    {
        public ConfigureEventArgs(Message model, EventContext context) : base(model, context)
        {
        }
    }
}
