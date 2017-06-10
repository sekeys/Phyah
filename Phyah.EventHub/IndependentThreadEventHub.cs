using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Concurrency;

namespace Phyah.EventHub
{
    public class IndependentThreadEventHub : EventHub
    {
        public IndependentThreadEventHub() : base(new IndependentThreadExecutor("IndependentThreadExecutor Hub", TimeSpan.Zero), new DefaultReceptorStore())
        {
        }
    }
}
