using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Phyah.EventPipeline
{
    using Phyah.Concurrency;
    public interface IEventPipeline: Phyah.Concurrency.IPipeline
    {
    }
}