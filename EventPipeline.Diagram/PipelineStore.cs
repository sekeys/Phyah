using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventPipeline.Diagram
{
    public abstract class PipelineStore
    {
        public IPipeline Match(IMatcher matcher)
        {
            throw new System.NotImplementedException();
        }
    }
}