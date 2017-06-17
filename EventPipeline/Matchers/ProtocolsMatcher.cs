using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventPipeline.Matchers
{
    public class ProtocolsMatcher : IMatcher
    {
        public Protocols Protocol { get; private set; }
        public ProtocolsMatcher(Protocols protocol)
        {
            Protocol = protocol;
        }
        public int Priority => 999;

        public bool Match(IEventPipeline pipeline)
        {
            throw new NotImplementedException();
        }
    }
}
