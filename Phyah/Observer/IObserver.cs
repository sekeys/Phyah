

namespace Phyah.Observer
{
    using Phyah.Interface;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public interface IObserver
    {
        void OnError(Exception ex);
        void Start(Input input);
        void Completed();
    }
}
