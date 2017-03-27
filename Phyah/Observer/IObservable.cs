using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Observer
{
    public interface IObservable
    {
        IDisposable Subscribe(IObserver observer);
    }
}
