using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventPipeline.Diagram
{
    public class ProtocolSelector : ISelector
    {
        public int Priority { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IMatcher Select()
        {
            throw new NotImplementedException();
        }
    }
}