using System;
using System.Collections.Generic;
using System.Text;
using Phyah.EventPipeline.Messages;

namespace Phyah.EventPipeline.Selectors
{
    public class UrlSelector : ISelector
    {
        public string Verbs { get ; set ; }
        public Protocols Protocol { get; set; }

        public int Priority => 999;

        public IEventPipeline Select(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
