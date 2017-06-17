using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventPipeline.Matchers
{
    public class TypeofMatcher : IMatcher
    {
        public TypeofMatcher(Type messageType,Type pipelineType=null)
        {
            MessageType = messageType;
            PipelineType = pipelineType;
        }
        public int Priority => 999;
        public Type MessageType { get; private set; }
        public Type PipelineType { get; private set; }
        public bool Match(IEventPipeline pipeline)
        {
            throw new NotImplementedException();
        }
    }
}
