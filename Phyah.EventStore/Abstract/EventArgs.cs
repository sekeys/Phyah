

namespace Phyah.EventStore
{
    using Phyah.EventStore.Messages;
    public abstract class EventArgs
    {
        public EventContext Context { get; private set; }
        public Message Message
        {
            get;
            private set;
        }
        public EventArgs(Message message, EventContext context)
        {
            this.Message = message;
            Context = context;
        }
        public EventArgs(EventContext context)
        {
            Context = context;
        }
    }
}