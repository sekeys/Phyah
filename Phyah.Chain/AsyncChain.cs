
namespace Phyah.Chain
{
    using System;
    using Phyah.Interface;
    using Phyah.Concurrency;
    using System.Threading.Tasks;
    using System.Threading;

    public class AsyncChain : IAsyncChain, IDisposable
    {
        const int CANCELED = 1;
        const int UNCANCELED = 0;
        protected int IsCanceled = 0;
        public bool Canceled => IsCanceled == CANCELED;
        protected readonly IExecutor ExecutorQueue;
        public AsyncChain()
        {
            //ExecutorQueue = new IndependentThreadExecutor("Phayh.ChainAsync", TimeSpan.FromMilliseconds(100));
            ExecutorQueue = new IndependentThreadExecutor("Phayh.ChainAsync", TimeSpan.FromMilliseconds(100));
        }

        public static IAsyncChain Links(IRunnable runnable)
        {
            return new AsyncChain().Link(runnable);
        }
        public static IAsyncChain Links(Action action) => Links(new Runnable(action));

        public static IAsyncChain Links(Action<object> action, object state) => Links(new StateRunnable(action, state));

        public static IAsyncChain Links(Action<object, object> action, object context, object state) => Links(new StateWithContextRunnable(action, context, state));
        public void Run()
        {
        }

        public IAsyncChain Link(IRunnable runnable)
        {
            if (ExecutorQueue.IsShutdown ||
                ExecutorQueue.IsShuttingDown
                || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }
            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.Schedule(runnable, TimeSpan.Zero);
            return this;
        }

        public IAsyncChain Link(Action action) => Link(new Runnable(action));

        public IAsyncChain Link(Action<object> action, object state) => Link(new StateRunnable(action, state));

        public IAsyncChain Link(Action<object, object> action, object context, object state) => Link(new StateWithContextRunnable(action, context, state));

        public IAsyncChain Link(IRunnable runnable, TimeSpan delay)
        {
            if (ExecutorQueue.IsShutdown ||
                   ExecutorQueue.IsShuttingDown
                   || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }
            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.Schedule(runnable, delay);
            return this;
        }

        public IAsyncChain Link(Action action, TimeSpan delay) => Link(new Runnable(action), delay);

        public IAsyncChain Link(Action<object> action, object state, TimeSpan delay) => Link(new StateRunnable(action, state), delay);

        public IAsyncChain Link(Action<object, object> action, object context, object state, TimeSpan delay) => Link(new StateWithContextRunnable(action, context, state), delay);


        public IAsyncChain Link(Action action, TimeSpan delay, CancellationToken cancellationToken)
        {
            if (ExecutorQueue.IsShutdown ||
                   ExecutorQueue.IsShuttingDown
                   || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }
            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.ScheduleAsync(action, TimeSpan.Zero, cancellationToken);
            return this;
        }

        public IAsyncChain Link(Action<object> action, object state, CancellationToken cancellationToken)
        {
            if (ExecutorQueue.IsShutdown ||
                   ExecutorQueue.IsShuttingDown
                   || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }

            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.ScheduleAsync(action, state, TimeSpan.Zero, cancellationToken);
            return this;
        }
        public IAsyncChain Link(Action<object, object> action, object context, object state, CancellationToken cancellationToken)
        {
            if (ExecutorQueue.IsShutdown ||
                   ExecutorQueue.IsShuttingDown
                   || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }

            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.ScheduleAsync(action, context, state, TimeSpan.Zero, cancellationToken);
            return this;
        }


        public IAsyncChain Link(Action action, CancellationToken cancellationToken, TimeSpan delay)
        {
            if (ExecutorQueue.IsShutdown ||
                   ExecutorQueue.IsShuttingDown
                   || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }
            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.ScheduleAsync(action, delay, cancellationToken);
            return this;
        }

        public IAsyncChain Link(Action<object> action, object state, CancellationToken cancellationToken, TimeSpan delay)
        {
            if (ExecutorQueue.IsShutdown ||
                   ExecutorQueue.IsShuttingDown
                   || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }
            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.ScheduleAsync(action, state, delay, cancellationToken);
            return this;
        }

        public IAsyncChain Link(Action<object, object> action, object context, object state, CancellationToken cancellationToken, TimeSpan delay)
        {
            if (ExecutorQueue.IsShutdown ||
                   ExecutorQueue.IsShuttingDown
                   || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }
            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.ScheduleAsync(action, context, state, delay, cancellationToken);
            return this;
        }

        public IAsyncChain Link(Action action, CancellationToken cancellationToken)
        {
            if (ExecutorQueue.IsShutdown ||
                      ExecutorQueue.IsShuttingDown
                      || ExecutorQueue.IsTerminated) { throw new ChainCancelException("Executor Queue has Cancel"); }
            if (Canceled)
            {
                throw new ChainCancelException();
            }
            ExecutorQueue.ScheduleAsync(action, TimeSpan.Zero, cancellationToken);
            return this;
        }

        public void Cancel()
        {
            if (Canceled)
            {
                return;
            }
            Interlocked.Exchange(ref IsCanceled, CANCELED);
            this.ExecutorQueue.ShutdownGracefullyAsync();
            Dispose();
        }

        public IAsyncChain CancelAsync(TimeSpan delay)
        {
            if (Canceled)
            {
                return this;
            }
            this.Link(() =>
            {
                if (Canceled)
                {
                    return;
                }
                Interlocked.Exchange(ref IsCanceled, CANCELED);
                this.ExecutorQueue.ShutdownGracefullyAsync();
                Dispose();

            }, delay);
            return this;
        }
        public IAsyncChain CancelAsync()
        {
            if (Canceled)
            {
                return this;
            }
            this.Link(() =>
            {
                if (Canceled)
                {
                    return;
                }
                Interlocked.Exchange(ref IsCanceled, CANCELED);
                this.ExecutorQueue.ShutdownGracefullyAsync(TimeSpan.Zero, TimeSpan.Zero).Wait();
                Dispose();

            });
            return this;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GC.Collect();
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~AsyncChain() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
