using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventStore
{
    using Phyah.TypeContainer;
    using Phyah.Extensions;
    public class GlobalVariable
    {
        private static bool HasInited = false;
        static object locker = new object();
        static GlobalVariable()
        {
            if (!HasInited)
            {
                lock (locker)
                {
                    if (HasInited)
                    {
                        return;
                    }
                    TypeContainer.Container.Inject(typeof(IHub), typeof(DefaultEventHub));
                    TypeContainer.Container.Inject(typeof(EventArgsFactory), typeof(DefaultEventHub));
                    TypeContainer.Container.Inject(typeof(ContextFactory), typeof(DefaultContextFactory));
                    TypeContainer.Container.Inject(typeof(EventStore), typeof(DefaultEventStore));
                    HasInited = true;
                }
            }
        }
        public static EventContext Context => Accessor<EventContext>.Current;
        public static EventArgsFactory EventArgsFactory
        {
            get => TypeContainer.Container.Construct<EventArgsFactory>();
        }

        public static ContextFactory ContextFactory
        {
            get => TypeContainer.Container.Construct<ContextFactory>();
        }

        public static IHub Hub
        {
            get => TypeContainer.Container.Construct<IHub>();
        }

        public static EventStore EventStore
        {
            get => TypeContainer.Container.Construct<EventStore>();
        }
    }
}