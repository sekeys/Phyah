using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventPipeline.Attribute
{
   public class TypeofPipelineAttribute:System.Attribute
    {
        public readonly Type PipelineType;
        public TypeofPipelineAttribute( Type pipelinetype)
        {
            this.PipelineType = pipelinetype;
        }
    }
}
