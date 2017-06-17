using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Concurrency
{
    public class ActionHandler : IHandler
    {
        readonly Action Action;
        public ActionHandler(Action action)
        {
            this.Action = action;
        }
        

        public void Handle()
        {
            Action.Invoke();
        }
    }
    public class ActionHandler<T> : IHandler<T>
    {
        readonly Action<T> Action;
        public ActionHandler(Action<T> action)
        {
            this.Action = action;
        }
        public void Handle(T value)
        {
             Action.Invoke(value);
        }
    }
}
