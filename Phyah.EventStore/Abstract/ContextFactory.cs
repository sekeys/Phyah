

namespace Phyah.EventStore
{
    public abstract class ContextFactory
    {
        public abstract EventContext Create(Messages.Message message);
    }
}