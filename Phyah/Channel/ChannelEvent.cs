using Phyah.Chain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Channel
{
    public class ChannelEvent
    {
        private readonly IExecutable _Executable;
        public IExecutable Executable { get => _Executable; }
        public ChannelEvent(IExecutable executable)
        {
            _Executable = executable;
        }

    }
}
