using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Interface
{
    public interface IPattern
    {
        IPattern Next { get;  }
        string Raw { get; }
        IPattern Previous { get; }
    }
}
