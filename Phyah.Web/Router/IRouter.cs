using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;

namespace Phyah.Web.Router
{
    public interface IRouter
    {
        int Priority { get; }
        IBehavior Next(IPath path);
        bool Match(IPath path);
    }
}
