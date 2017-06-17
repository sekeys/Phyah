using Phyah.EventPipeline.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventPipeline
{
    using Phyah.EventPipeline.Matchers;
    using System.Threading.Tasks;

    public class Acceptor
    {
        public void Accept(Message message)
        {
            var matchers = PipelineSelector.Selector.Select(message);
            PipelineMatcher pipelineMatcher = null;
            var pipelines = pipelineMatcher.Match(matchers);
            var pipeline = pipelineMatcher.SelectBest(pipelines);
            pipeline.Start();
            pipeline.Wait();
        }
        public Task AcceptAsync(Message message)
        {
            var matchers = PipelineSelector.Selector.Select(message);
            PipelineMatcher pipelineMatcher = null;
            var pipelines = pipelineMatcher.Match(matchers);
            var pipeline = pipelineMatcher.SelectBest(pipelines);
            pipeline.Start();
            return pipeline.Task;
        }

    }
}