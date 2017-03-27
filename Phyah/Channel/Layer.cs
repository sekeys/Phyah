

namespace Phyah.Channel
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Phyah.Chain;
    using System.Threading;
    public abstract class Channel : IChannel
    {
        //protected readonly ExecutableChain Chain;
        //public Channel(ExecutableChain chain)
        //{
        //    this.Chain = chain;
        //}
        //public Channel()
        //{
        //    Chain = new ExecutableChain();
        //}
        private int isStopped;
        public void OnCompleted()
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
                throw ex;
            }
        }

        public void OnStart()
        {
            if (Volatile.Read(ref isStopped) == 0)
                OnStartCore();
        }
        protected abstract void CompletedCore();

        protected abstract void OnErrorCore(Exception ex);

        protected abstract void OnStartCore();
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

        public virtual IChannel Subscribe(IExecutable executable)
        {
            Require.NotNull(executable);
            //Chain.Enqueue(executable);
            return this;
        }
    }
}
