using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventStore.Attributes
{
    using Phyah;
    using Phyah.Concurrency;

    public class AuthenticationAttribute:Attribute
    {
        public IAuthentication Authentication;
        public AuthenticationAttribute(IAuthentication authentication)
        {
            Authentication = authentication;
        }
    }
}