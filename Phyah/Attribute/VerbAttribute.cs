

namespace Phyah.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Phyah.Enumerable;
    public class VerbAttribute:Attribute
    {
        public Verbs Verb { get; private set; }
        public VerbAttribute(Verbs verb)
        {
            Verb = verb;
        }
    }
}
