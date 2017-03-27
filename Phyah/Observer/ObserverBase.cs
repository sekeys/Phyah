

namespace Phyah.Observer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Phyah.Interface;
    using System.Threading;
    public abstract class ObserverBase : IObserver,IDisposable
    {
        private int isStopped;
        public void Completed()
        {
            if (Interlocked.Exchange(ref isStopped, 1) == 0)
            {
                CompletedCore();
            }
        }

        public void OnError(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            if (Interlocked.Exchange(ref isStopped, 1) == 0)
            {
                OnErrorCore(ex);
            }
        }

        public void Start(Input input)
        {
            if (Volatile.Read(ref isStopped) == 0)
                StartCore(input);
        }
        protected abstract void CompletedCore();

        protected abstract void OnErrorCore(Exception ex);

        protected abstract void StartCore(Input input);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Volatile.Write(ref isStopped, 1);
            }
        }
        internal bool Fail(Exception error)
        {
            if (Interlocked.Exchange(ref isStopped, 1) == 0)
            {
                OnErrorCore(error);
                return true;
            }
            return false;
        }
    }
}
