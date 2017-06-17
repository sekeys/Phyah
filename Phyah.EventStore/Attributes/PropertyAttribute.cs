using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventStore.Attributes
{
    public class PropertyAttribute:Attribute
    {
        public string Property { get; set; }
        public object Value { get; set; }
        public PropertyAttribute(string property,object value)
        {
            this.Property = property;
            Value = value;
        }
        public PropertyAttribute()
        {
        }
    }
}
