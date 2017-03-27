

namespace Phyah.Observer
{
    using Phyah.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class Variable
    {
        public static readonly Action Nop = () => { };
        public static readonly Action<Exception> Throw = ex => { ex.Throw(); };

    }
    
}
