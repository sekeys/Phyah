

namespace Phyah.Chain
{
    using Phyah.Interface;
    using System;
    #region 
    public sealed class Runnable : IRunnable
    {
        public Runnable(Action action)
        {
            this.action = action;
        }
        readonly Action action;
        public void Run()
        {
            action();
        }
    }
    public sealed class StateRunnable : IRunnable
    {
        readonly object State;
        public StateRunnable(Action<object> action, object state)
        {
            this.action = action;
            State = state;
        }
        readonly Action<object> action;
        public void Run()
        {
            action(State);
        }
    }
    public sealed class StateWithContextRunnable : IRunnable
    {
        readonly object State;
        readonly object Context;
        public StateWithContextRunnable(Action<object, object> action, object context, object state)
        {
            this.action = action;
            State = state;
            Context = context;
        }
        readonly Action<object, object> action;
        public void Run()
        {
            action(Context, State);
        }
    }
    #endregion
}
