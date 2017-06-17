using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web.Razor
{
    public interface IRazorCompileService
    {

        Type Compile(string code);

    }
}
