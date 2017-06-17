

namespace Phyah.Web
{
    using Phyah.Concurrency;
    using Phyah.Web.Handler;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    public class Initialization
    {
        public static Phyah.Concurrency.IPipeline Pipeline { get; private set; }
        public static void Initialize()
        {
            Thread.CurrentThread.Name = "Phyah.Master";
            Phyah.Web.BehaviorFactory.Register(new DefaultBehaviorFactory());
        }
        public static void InitializePipeline(IPipeline pipeline)
        {
            Pipeline = pipeline;
        }
    }

}
