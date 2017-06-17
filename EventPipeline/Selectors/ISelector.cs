using Phyah.EventPipeline.Matchers;


namespace Phyah.EventPipeline
{
    using Phyah.EventPipeline.Messages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public interface ISelector
    {

        IEnumerable<IMatcher> Select(Message message);
    }
}