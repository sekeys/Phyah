using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;

namespace Phyah.Observer
{
    public abstract class SubjectBase : ISubject
    {
        public abstract bool HasObservers { get; }
        public abstract bool IsDisposed { get; }
        public abstract void Completed();
        public abstract void Dispose();

        public abstract void OnError(Exception ex);
        public abstract void Start(Input input);

        public abstract IDisposable Subscribe(IObserver observer);
    }
}
