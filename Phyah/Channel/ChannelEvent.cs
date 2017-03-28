using Phyah.Chain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Channel
{
    public class ChannelEvent
    {
        public readonly string SourceName {get;set;}
        public ChannelEvent(string sourceName)
        {
            SourceName=sourceName;
        }

    }
}
