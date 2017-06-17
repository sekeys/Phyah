using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventStore.Attributes
{
    public class FilterAttribute:Attribute
    {
        public IFilter Filter { get; private set; }
        public FilterAttribute(Type type)
        {
            this.Filter = Activator.CreateInstance(type) as IFilter;
        }
    }
}