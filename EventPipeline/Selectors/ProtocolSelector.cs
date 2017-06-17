
namespace Phyah.EventPipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Phyah.EventPipeline.Messages;
    using Phyah.EventPipeline.Matchers;

    public class ProtocolSelector : ISelector
    {

        public IEnumerable<IMatcher> Select(Message message)
        {
            var ls = new List<IMatcher>();
            ls.Add(new ProtocolsMatcher(message.Protocol));
            if (message.Protocol == Protocols.Http && message is HttpMessage && message.Data.Exist("url"))
            {
                ls.Add(new UrlMatcher(message.Data.Url));
            }
            return ls;
        }
    }
}