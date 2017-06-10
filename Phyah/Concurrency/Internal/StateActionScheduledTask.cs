using System;
using Phyah.Interface;

namespace Phyah.Concurrency
{
    using System;

    sealed class StateActionScheduledTask : ScheduledTask
    {
        readonly Action<object> action;

        public StateActionScheduledTask(Executor executor, Action<object> action, object state, PreciseTimeSpan deadline)
            : base(executor, deadline, new TaskCompletionSource(state))
        {
            this.action = action;
        }

        protected override void Execute() => this.action(this.Completion.AsyncState);
    }
}