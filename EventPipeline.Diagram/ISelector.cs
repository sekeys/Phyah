using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventPipeline.Diagram
{
    public interface ISelector
    {
        int Priority { get; set; }

        IMatcher Select();
    }
}