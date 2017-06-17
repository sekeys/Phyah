using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore
{
    public class DefaultContextFactory : ContextFactory
    {
        public override EventContext Create(Messages.Message message)
        {
            EventContext context = new DefaultEventContext();

            Accessor<EventContext>.Current = context;
            return context;
        }
    }
}
