using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Interface
{
    public interface IPriorityRunnable:IRunnable
    {
        int Priority { get; }
    }
}
