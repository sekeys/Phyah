

namespace Phyah.EventPipeline.Attribute
{
    using Phyah.EventPipeline.Matchers;

    public abstract class PipelineConstructorAttribute : System.Attribute
    {
        public PipelineConstructorAttribute()
        {

        }
        public abstract IEventPipeline Construct(Messages.Message message);
    }
}
