

namespace Phyah.EventPipeline.Attribute
{
    using Phyah.EventPipeline.Matchers;

    public abstract class MatcherConstructorAttribute:System.Attribute
    {
        public MatcherConstructorAttribute()
        {

        }
        public abstract IMatcher Construct(Messages.Message message);
    }
}
