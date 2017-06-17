

namespace Phyah.Web.Handler
{
    using Phyah.Concurrency;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class ProcessHandler : WebHandler
    {
        protected override void HandleCore()
        {
            bool debug = AccessorContext.DefaultContext.Get<bool>("debug");
            Processor processor = null;
            if (!debug)
            {
                processor = new DefaultProcessor();
            }
            else
            {
                processor = new DebugProcessor();
            }
            processor.Process();
            //System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[] { task });
        }
    }
}
