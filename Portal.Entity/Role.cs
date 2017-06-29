using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Entity
{
    public class Role:BaseEntity
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
    }
}
