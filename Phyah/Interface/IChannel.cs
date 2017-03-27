using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Interface
{
    public interface IChannel:IDisposable
    {
        void Start();
        IChannel Subscribe(IChannel channel);
        void Completed();
    }
}
