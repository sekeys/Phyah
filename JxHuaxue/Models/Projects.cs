using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Models
{
    public class Projects:IEntity
    {
        public string Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string Datetime { get; set; }
        public string ImageSrc { get; set; }
        public string Href { get; set; }
    }
}
