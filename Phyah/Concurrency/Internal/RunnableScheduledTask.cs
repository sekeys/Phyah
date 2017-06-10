using Phyah.Interface;

namespace Phyah.Concurrency
{
    sealed class RunnableScheduledTask : ScheduledTask
    {
        readonly IRunnable action;

        public RunnableScheduledTask(Executor executor, IRunnable action, PreciseTimeSpan deadline)
            : base(executor, deadline, new TaskCompletionSource())
        {
            this.action = action;
        }

        protected override void Execute() => this.action.Run();
    }
}