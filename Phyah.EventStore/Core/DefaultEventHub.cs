using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phyah.Concurrency;

namespace Phyah.EventStore
{
    public class DefaultEventHub : EventHub
    {
        public DefaultEventHub() : base(new IndependentThreadExecutor(new MultiExecutor(2),"DefaultEventHub",TimeSpan.Zero)
            , new IndependentThreadExecutor(new MultiExecutor(2), "DefaultEventHub", TimeSpan.Zero))
        {
        }
    }
}