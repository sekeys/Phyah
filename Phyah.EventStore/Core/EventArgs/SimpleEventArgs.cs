using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore
{
    public class SimpleEventArgs:EventArgs
    {
        public SimpleEventArgs(Messages.Message message, EventContext context) : base(message, context)
        {

        }
        public object Data { get; set; } 
    }
}
