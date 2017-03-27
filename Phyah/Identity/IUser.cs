using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Identity
{
    public interface IUser : System.Security.Principal.IIdentity
    {
        string Token { get; }
        List<string> Roles
        {
            get;
        }
    }
}
