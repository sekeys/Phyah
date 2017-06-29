using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Concurrency
{
    public static class IPipelineExtensions
    {
        internal class ActionHandler : IHandler
        {
            readonly Action Action;
            public ActionHandler(Action action)
            {
                Action = action;
            }
            public void Handle()
            {
                Action.Invoke();
            }
        }
        public static IPipeline AddLast(this IPipeline pipeline, Action action)
        {

            return pipeline.AddLast(new ActionHandler(action));
        }
        public static IPipeline AddLast(this IPipeline pipeline, string name, Action action)
        {

            return pipeline.AddLast(name, new ActionHandler(action));
        }
    }
}
