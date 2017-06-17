using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore.Enumerate
{
    public enum EventMode
    {
        Emit,
        Register,
        Subscribe,
        Upgrade
    }
}
