

namespace Phyah.Authorization
{
    using System.Collections.Generic;
    public interface IUser
    {
        string UserName { get; }
        IEnumerable<string> Roles { get; }

    }
}
