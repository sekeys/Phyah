using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;
using System.Threading;

namespace Phyah.Observer
{
    public class Subject : SubjectBase
    {
        bool isDisposed;
        bool isStopped;
        ImmutableList<IObserver> observers;
        object gate = new object();
        Exception exception;
        public override bool HasObservers => throw new NotImplementedException();

        public override bool IsDisposed => throw new NotImplementedException();

        public override void Completed()
        {
            var os = default(IObserver[]);
            lock (gate)
            {
                if (!isStopped)
                {
                    os = observers.Data;
                    observers = new ImmutableList<IObserver>();
                    isStopped = true;
                }
            }
            if (os != null)
            {
                foreach (var o in os)
                {
                    o.Completed();
                }
            }
        }
        

        public override void OnError(Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException("error");

            var os = default(IObserver[]);
            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    os = observers.Data;
                    observers = new ImmutableList<IObserver>();
                    isStopped = true;
                    exception = ex;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.OnError(ex);
        }

        public override void Start(Input input)
        {
            var os = default(IObserver[]);
            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    os = observers.Data;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.Start(input);
        }
        void Unsubscribe(IObserver observer)
        {
            lock (gate)
            {
                if (observers != null)
                    observers = observers.Remove(observer);
            }
        }
        public override IDisposable Subscribe(IObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            lock (gate)
            {
                CheckDisposed();

                if (!isStopped)
                {
                    observers = observers.Add(observer);
                    return new Subscription(this, observer);
                }
                else if (exception != null)
                {
                    observer.OnError(exception);
                    return default(IDisposable);
                }
                else
                {
                    observer.Completed();
                    return default(IDisposable);
                }
            }
        }
        class Subscription : IDisposable
        {
            Subject subject;
            IObserver observer;

            public Subscription(Subject subject, IObserver observer)
            {
                this.subject = subject;
                this.observer = observer;
            }

            public void Dispose()
            {
                var o = Interlocked.Exchange<IObserver>(ref observer, null);
                if (o != null)
                {
                    subject.Unsubscribe(o);
                    subject = null;
                }
            }
        }
        void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(string.Empty);
        }

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public override void Dispose()
        {
            lock (gate)
            {
                isDisposed = true;
                observers = null;
            }
        }
        
    }
}
