using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore.Attributes
{
    public class EventArgsAttribute:Attribute
    {
        public EventArgs Create()
        {
            return null;
        }
    }
}
