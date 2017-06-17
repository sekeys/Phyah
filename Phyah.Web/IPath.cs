using System;
using System.Collections.Generic;
using System.Text;
using Phyah;

namespace Phyah.Interface
{
    public interface IPath
    {
        string Raw { get; set; }
        bool Next();
        IPattern Previous();
        IPattern Current
        {
            get;
        }
        IPattern Pattern { get; }

    }
}
