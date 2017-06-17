using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore
{
    public interface IAuthentication
    {
        bool Authenticate(EventContext context);
    }
}
