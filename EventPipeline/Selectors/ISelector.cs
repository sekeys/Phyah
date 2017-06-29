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
        string Verbs { get; set; }
        Protocols Protocol { get; set; }
        int Priority { get; }
        //IEnumerable<IMatcher> Select(Message message);
        IEventPipeline Select(Message message);
    }
}