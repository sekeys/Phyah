using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventPipeline.Matchers
{
    public interface IMatcher
    {
        int Priority { get; }
        bool Match(IEventPipeline pipeline);
    }
}