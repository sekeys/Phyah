using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;

namespace Phyah.Observer
{
    public class SynchronizedObserver : ObserverBase
    {
        private readonly object _gate;
        private readonly IObserver _observer;

        public SynchronizedObserver(IObserver observer, object gate)
        {
            _gate = gate;
            _observer = observer;
        }
        
        protected override void OnErrorCore(Exception exception)
        {
            lock (_gate)
            {
                _observer.OnError(exception);
            }
        }

        protected override void CompletedCore()
        {
            lock (_gate)
            {
                _observer.Completed();
            }
        }

        protected override void StartCore(Input input)
        {
            lock (_gate)
            {
                _observer.Start(input);
            }
        }
    }
}
