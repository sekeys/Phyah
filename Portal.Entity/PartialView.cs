using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Entity
{
    public class PartialView:BaseEntity
    {
        public string Hash { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string Content { get; set; }
    }
}
