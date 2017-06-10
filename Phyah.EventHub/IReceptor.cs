using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventHub
{
    public interface IReceptor
    {
        void Accept(IEvent evnt);
    }
}
