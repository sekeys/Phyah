

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
        public IEnumerable<IMatcher> Select(Message message)
        {
            throw new NotImplementedException();
        }
    }
}