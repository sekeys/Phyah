using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventStore
{
    public interface IFilter
    {
        bool Filter(EventContext context);
    }
}