

namespace Phyah. EventPipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Phyah.EventPipeline.Matchers;
    using Phyah.EventPipeline.Messages;
    public class GateWaySelector : ISelector
    {
        public string Verbs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Protocols Protocol { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Priority => throw new NotImplementedException();

        public IEventPipeline Select(Message message)
        {
            throw new NotImplementedException();
        }
    }
}