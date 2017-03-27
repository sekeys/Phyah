using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Observer
{
    public  class AnonymouseObservable: IObservable
    {
        private readonly Func<IObserver, IDisposable> _subscribe;
        public IDisposable Subscribe(IObserver observer)
        {
            Require.NotNull(observer);
            return _subscribe(observer);
        }
        
    }
}
