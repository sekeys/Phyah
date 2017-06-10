

namespace Phyah.Chain
{
    using Phyah.Interface;
    using System;
    #region 

    public sealed class PriorityRunnable : IPriorityRunnable
    {
        public int Priority { get; private set; }
        public PriorityRunnable(IRunnable runnable, int priority)
        {
            this.action = runnable;
            Priority = priority;
        }

        public PriorityRunnable(IRunnable runnable)
        {
            this.action = runnable;
            Priority = 0;
        }
        readonly IRunnable action;
        public void Run()
        {
            action.Run();
        }
    }
    public sealed class PriorityActionRunnable : IPriorityRunnable
    {
        public int Priority { get; private  set; }
        public PriorityActionRunnable(Action action,int priority)
        {
            this.action = action;
            Priority = priority;
        }

        public PriorityActionRunnable(Action action)
        {
            this.action = action;
            Priority = 0;
        }
        readonly Action action;
        public void Run()
        {
            action();
        }
    }
    public sealed class PriorityStateRunnable : IPriorityRunnable
    {
        readonly object State;
        public int Priority { get; private set; }
        public PriorityStateRunnable(Action<object> action, object state, int priority)
        {
            this.action = action;
            State = state;
            Priority = priority;
        }
        public PriorityStateRunnable(Action<object> action, object state)
        {
            this.action = action;
            State = state;
            Priority = 0;
        }
        readonly Action<object> action;
        public void Run()
        {
            action(State);
        }
    }
    public sealed class PriorityStateWithContextRunnable : IPriorityRunnable
    {
        readonly object State;
        readonly object Context;
        public int Priority { get; private set; }
        public PriorityStateWithContextRunnable(Action<object, object> action, object context, object state, int priority)
        {
            this.action = action;
            State = state;
            Context = context;
            Priority = priority;
        }
        public PriorityStateWithContextRunnable(Action<object, object> action, object context, object state)
        {
            this.action = action;
            State = state;
            Context = context;
            Priority = 0;
        }
        readonly Action<object, object> action;
        public void Run()
        {
            action(Context, State);
        }
    }
    #endregion
}
