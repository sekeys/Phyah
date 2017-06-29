using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Models
{
    public class PageModuleEntity
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string PageId { get; set; }
        public string Name { get; set; }
        public string Html { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public DateTime CreateTime { get; set; }

    }
}
