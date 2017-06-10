using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Attributes
{
    public class DescriptionAttribute:Attribute
    {
        public string Description { get;private set; }
        public DescriptionAttribute(string description)
        {
            Description = description;
        }

    }
}
