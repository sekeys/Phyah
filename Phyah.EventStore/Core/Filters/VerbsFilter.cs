using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore.Core.Filters
{
    public class VerbsFilter : IFilter
    {
        public string Verbs { get;private set; }
        public VerbsFilter(string verbs)
        {
            Verbs = verbs;
        }
        public bool Filter(EventContext context)
        {
            return context.Message.Verb.Equals(Verbs);
        }
    }
}
