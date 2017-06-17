

namespace Phyah.EventPipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Phyah.EventPipeline.Matchers;
    using Phyah.EventPipeline.Messages;
    public class ContentSelector : ISelector
    {
        public IEnumerable<IMatcher> Select(Message message)
        {
            throw new NotImplementedException();
        }
    }
}