using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Channel
{
    public abstract class OutputChannel:Channel
    {
        public abstract Output Output { get; }
        public abstract void End();
    }
}
