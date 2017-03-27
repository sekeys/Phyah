using Phyah.Extensions;
using Phyah.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Phyah.Observer
{
    public sealed class AsyncSubject : SubjectBase, IDisposable//, INotifyCompletion
    {
        #region Fields

        private readonly object _gate = new object();

        private ImmutableList<IObserver> _observers;
        private bool _isDisposed;
        private bool _isStopped;
        private Input _value;
        private bool _hasValue;
        private Exception _exception;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a subject that can only receive one value and that value is cached for all future observations.
        /// </summary>
        public AsyncSubject()
        {
            _observers = ImmutableList<IObserver>.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public override bool HasObservers
        {
            get
            {
                var observers = _observers;
                return observers != null && observers.Data.Length > 0;
            }
        }

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
        public override bool IsDisposed
        {
            get
            {
                lock (_gate)
                {
                    return _isDisposed;
                }
            }
        }

        #endregion

        #region Methods

        #region IObserver<T> implementation

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence, also causing the last received value to be sent out (if any).
        /// </summary>
        public override void Completed()
        {
            var os = default(IObserver[]);

            var v = default(Input);
            var hv = false;
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    os = _observers.Data;
                    _observers = ImmutableList<IObserver>.Empty;
                    _isStopped = true;
                    v = _value;
                    hv = _hasValue;
                }
            }

            if (os != null)
            {
                if (hv)
                {
                    foreach (var o in os)
                    {
                        o.Start(v);
                        o.Completed();
                    }
                }
                else
                    foreach (var o in os)
                        o.Completed();
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the exception.
        /// </summary>
        /// <param name="error">The exception to send to all observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public override void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            var os = default(IObserver[]);
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    os = _observers.Data;
                    _observers = ImmutableList<IObserver>.Empty;
                    _isStopped = true;
                    _exception = error;
                }
            }

            if (os != null)
                foreach (var o in os)
                    o.OnError(error);
        }

        /// <summary>
        /// Sends a value to the subject. The last value received before successful termination will be sent to all subscribed and future observers.
        /// </summary>
        /// <param name="value">The value to store in the subject.</param>
        public override void Start(Input value)
        {
            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    _value = value;
                    _hasValue = true;
                }
            }
        }

        #endregion

        #region IObservable<T> implementation

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns>Disposable object that can be used to unsubscribe the observer from the subject.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public override IDisposable Subscribe(IObserver observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var ex = default(Exception);
            var v = default(Input);
            var hv = false;

            lock (_gate)
            {
                CheckDisposed();

                if (!_isStopped)
                {
                    _observers = _observers.Add(observer);
                    return new Subscription(this, observer);
                }

                ex = _exception;
                hv = _hasValue;
                v = _value;
            }

            if (ex != null)
            {
                observer.OnError(ex);
            }
            else if (hv)
            {
                observer.Start(v);
                observer.Completed();
            }
            else
            {
                observer.Completed();
            }

            return default(IDisposable);
        }

        class Subscription : IDisposable
        {
            private readonly AsyncSubject _subject;
            private IObserver _observer;

            public Subscription(AsyncSubject subject, IObserver observer)
            {
                _subject = subject;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null)
                {
                    lock (_subject._gate)
                    {
                        if (!_subject._isDisposed && _observer != null)
                        {
                            _subject._observers = _subject._observers.Remove(_observer);
                            _observer = null;
                        }
                    }
                }
            }
        }

        #endregion

        #region IDisposable implementation

        void CheckDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(string.Empty);
        }

        /// <summary>
        /// Unsubscribe all observers and release resources.
        /// </summary>
        public override void Dispose()
        {
            lock (_gate)
            {
                _isDisposed = true;
                _observers = null;
                _exception = null;
                _value = default(Input);
            }
        }

        #endregion

        #region Await support

        /// <summary>
        /// Gets an awaitable object for the current AsyncSubject.
        /// </summary>
        /// <returns>Object that can be awaited.</returns>
        public AsyncSubject GetAwaiter()
        {
            return this;
        }

        /// <summary>
        /// Specifies a callback action that will be invoked when the subject completes.
        /// </summary>
        /// <param name="continuation">Callback action that will be invoked when the subject completes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="continuation"/> is null.</exception>
        public void OnCompleted(Action continuation)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));

            OnCompleted(continuation, true);
        }

        private void OnCompleted(Action continuation, bool originalContext)
        {
            this.Subscribe/*Unsafe*/(new AwaitObserver(continuation, originalContext));
        }

        class AwaitObserver : IObserver
        {
            private readonly SynchronizationContext _context;
            private readonly Action _callback;

            public AwaitObserver(Action callback, bool originalContext)
            {
                if (originalContext)
                    _context = SynchronizationContext.Current;

                _callback = callback;
            }

            public void Completed()
            {
                InvokeOnOriginalContext();
            }

            public void OnError(Exception error)
            {
                InvokeOnOriginalContext();
            }

            public void Start(Input value)
            {
            }

            private void InvokeOnOriginalContext()
            {
                if (_context != null)
                {
                    _context.Post(c => ((Action)c)(), _callback);
                }
                else
                {
                    _callback();
                }
            }
        }

        /// <summary>
        /// Gets whether the AsyncSubject has completed.
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return _isStopped;
            }
        }

        /// <summary>
        /// Gets the last element of the subject, potentially blocking until the subject completes successfully or exceptionally.
        /// </summary>
        /// <returns>The last element of the subject. Throws an InvalidOperationException if no element was received.</returns>
        /// <exception cref="InvalidOperationException">The source sequence is empty.</exception>
        public Input GetResult()
        {
            if (!_isStopped)
            {
                var e = new ManualResetEvent(false);
                OnCompleted(() => e.Set(), false);
                e.WaitOne();
            }

            _exception.ThrowIfNotNull();

            if (!_hasValue)
                throw new InvalidOperationException("No Element");

            return _value;
        }

        #endregion
        #endregion
    }
}
