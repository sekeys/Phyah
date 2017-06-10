

namespace Phyah.Concurrency
{
    using Phyah.Interface;
    using System;

    public interface IScheduledRunnable : IRunnable, IScheduledTask, IComparable<IScheduledRunnable>
    {
    }
}