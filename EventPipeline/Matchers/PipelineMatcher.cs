using Phyah.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventPipeline.Matchers
{
    public class PipelineMatcher
    {
        class MatcherCompare : IComparer<IMatcher>
        {
            public int Compare(IMatcher x, IMatcher y)
            {
                return x.Priority - y.Priority;
            }
        }
        readonly PriorityQueue<IMatcher> matcher;
        public PipelineMatcher()
        {
            matcher= new PriorityQueue<IMatcher>(new MatcherCompare());
        }
        public IEnumerable<IEventPipeline> Match(IEnumerable<IMatcher> matchers)
        {
            return null;
        }
        public IEventPipeline SelectBest(IEnumerable<IEventPipeline> pipelines)
        {
            return null;
        }
    }
}