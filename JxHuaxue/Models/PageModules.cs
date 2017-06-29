using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Models
{
    [Table("page_rel_module")]
    public class PageModules : IEntity
    {
        public string Id { get; set; }
        public string PageId { get; set; }
        public string Children { get; set; }
        public int Index { get; set; }
    }
}
