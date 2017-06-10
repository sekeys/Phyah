using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Concurrency
{
    public enum PipelineState
    {
        Unstarted,
        Running,
        Completed,
        Exception,
        Canceled,
    }
}
