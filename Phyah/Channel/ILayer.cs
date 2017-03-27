

namespace Phyah.Channel
{
    using Phyah.Chain;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public interface IChannel
    {
        void OnStart();
        void OnCompleted();

        IChannel Subscribe(IExecutable executable);
    }
}
