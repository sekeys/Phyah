

namespace Phyah.Chain
{
    using Phyah.Interface;
    using System;
    public interface IPriorityChain:IChain 
    {
        IChain Link(IRunnable runnable,int priority);
        IChain Link(Action action, int priority);
        IChain Link(Action<object> action, object state, int priority);
        IChain Link(Action<object, object> action, object context, object state, int priority);
    }
}
