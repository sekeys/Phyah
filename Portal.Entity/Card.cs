using Phyah.EntityFramework.Services;
using System;

namespace Portal.Entity
{
    public class Card : BaseEntity
    {
        public string CardNo { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
        public string ImageSource { get; set; }
        public string Remark { get; set; }
        public string Classify { get; set; }
    }
}
