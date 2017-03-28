using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Channel
{
    public abstract class ChannelReceptor
    {
        public abstract void Accept(ChannelEvent e);
    }
}
