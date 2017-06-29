using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Models
{
    public class Card : IEntity
    {
        public string Id { get; set; }

        public string CardNo { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
        public string ImageSource { get; set; }
        public string Remark { get; set; }
        public bool Enable { get; set; }
    }
}
