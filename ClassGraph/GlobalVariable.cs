using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public class GlobalVariable
    {
        public EventArgsFactory EventArgsFactory
        {
            get => default(int);
            set
            {
            }
        }

        public ContextFactory ContextFactory
        {
            get => default(int);
            set
            {
            }
        }

        public IHub Hub
        {
            get => default(int);
            set
            {
            }
        }

        public IEventStore EventStore
        {
            get => default(int);
            set
            {
            }
        }
    }
}