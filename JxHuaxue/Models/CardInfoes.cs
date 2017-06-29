using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Models
{
    public class CardInfoes:IEntity
    {
        public string Id { get; set; }
        public string CardNo { get; set; }
        public string CardInfo { get; set; }
        public string Description { get; set; }
        public string ImgSrc { get; set; }
        public string Href { get; set; }
    }
}
