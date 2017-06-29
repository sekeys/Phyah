using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Entity
{
    public class Contactor:BaseEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Remark { get; set; }
        public string Information { get; set; }
        public string FormCode { get; set; }
        public DateTime SubtmitDate { get; set; }

    }
}
