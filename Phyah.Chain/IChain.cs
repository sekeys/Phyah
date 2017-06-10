

namespace Phyah.Chain
{
    using Phyah.Interface;
    using System;
    public interface IChain
    {
        IChain Link(IRunnable runnable);
        IChain Link(Action action);
        IChain Link(Action<object> action, object state);
        IChain Link(Action<object, object> action, object context, object state);
        void Cancel();
        //void Link(Action<object, object, object> action);
        //void Link(Action<object, object, object, object> action);
        //void Link(Action<object, object, object, object, object> action);
        //void Link(Action<object, object, object, object, object, object> action);
        //void Link(Action<object, object, object, object, object, object, object> action);
    }
}
