using Phyah.EventStore.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore
{
    public class AssemlyEventArgs:EventArgs
    {
        public AssemlyEventArgs(Message model, EventContext context) : base(model, context)
        { 
        }
    }
}
