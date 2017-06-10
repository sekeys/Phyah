

namespace Phyah.Chain
{
    using Phyah.Interface;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAsyncChain
    {
        IAsyncChain Link(IRunnable runnable);
        IAsyncChain Link(Action action);
        IAsyncChain Link(Action<object> action, object state);
        IAsyncChain Link(Action<object,object> action, object context, object state);

        IAsyncChain Link(IRunnable runnable,TimeSpan delay);
        IAsyncChain Link(Action action, TimeSpan delay);
        IAsyncChain Link(Action<object> action, object state, TimeSpan delay);
        IAsyncChain Link(Action<object, object> action, object context, object state, TimeSpan delay);


        //IAsyncChain Link(IRunnable runnable, CancellationToken cancellationToken);
        IAsyncChain Link(Action action, CancellationToken cancellationToken);
        IAsyncChain Link(Action<object> action, object state, CancellationToken cancellationToken);
        IAsyncChain Link(Action<object, object> action, object context, object state, CancellationToken cancellationToken);
        //IAsyncChain Link(IRunnable runnable, TimeSpan delay, CancellationToken cancellationToken);
        IAsyncChain Link(Action action, CancellationToken cancellationToken, TimeSpan delay);
        IAsyncChain Link(Action<object> action, object state, CancellationToken cancellationToken, TimeSpan delay);
        IAsyncChain Link(Action<object, object> action, object context, object state, CancellationToken cancellationToken, TimeSpan delay);
        IAsyncChain CancelAsync();
        IAsyncChain CancelAsync(TimeSpan delay);
        void Cancel();
        //void Link(Action<object, object, object> action);
        //void Link(Action<object, object, object, object> action);
        //void Link(Action<object, object, object, object, object> action);
        //void Link(Action<object, object, object, object, object, object> action);
        //void Link(Action<object, object, object, object, object, object, object> action);
    }
}
