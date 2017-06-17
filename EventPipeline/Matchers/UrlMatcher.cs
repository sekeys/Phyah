using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventPipeline.Matchers
{
    public class UrlMatcher : IMatcher
    {
        public readonly string Url;
        public UrlMatcher(string url)
        {
            Url = url;
        }

        public int Priority => 999;

        public bool Match(IEventPipeline pipeline)
        {
            throw new NotImplementedException();
        }
    }
}