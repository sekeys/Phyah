

namespace Phyah.Chain
{
    using Phyah.Collection;
    using Phyah.Interface;
    using System;
    using System.Threading;

    public class Chain : IChain,IDisposable
    {

        const int CANCELED = 1;
        const int UNCANCELED = 0;
        protected int IsCanceled = 0;
        public bool Canceled => IsCanceled == CANCELED;

        public static IChain Links(IRunnable runnable)
        {
            return new Chain().Link(runnable);
        }
        public static IChain Links(Action action) => Links(new Runnable(action));

        public static IChain Links(Action<object> action, object state) => Links(new StateRunnable(action, state));

        public static IChain Links(Action<object, object> action, object context, object state) => Links(new StateWithContextRunnable(action, context, state));

        readonly IConcurrentQueue<IRunnable> Queue;
        public Chain()
        {
            Queue = new FriendlyConcurrentQueue<IRunnable>();
        }

        public void Run()
        {
            if (Queue.IsEmpty) { return; }
            IRunnable task = default(IRunnable);
            if (Queue.TryDequeue(out task))
            {
                while (task != null)
                {
                    task.Run();
                    Queue.TryDequeue(out task);
                }
            }
        }

        public IChain Link(IRunnable runnable)
        {
            Queue.TryEnqueue(runnable);
            return this;
        }

        public IChain Link(Action action) => Link(new Runnable(action));

        public IChain Link(Action<object> action,object state) => Link(new StateRunnable(action, state));

        public IChain Link(Action<object, object> action, object context, object state) => Link(new StateWithContextRunnable(action, context, state));

        public void Cancel()
        {
            if (Canceled)
            {
                return;
            }
            Interlocked.Exchange(ref IsCanceled, CANCELED);
            Dispose();
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
        //public void Link(Action<object, object, object> action, object obj3, object obj)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Link(Action<object, object, object, object> action)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Link(Action<object, object, object, object, object> action)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Link(Action<object, object, object, object, object, object> action)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Link(Action<object, object, object, object, object, object, object> action)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
