using Phyah.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Observer
{
    public interface ISubject : IObservable, IObserver, IDisposable
    {
    }
}
