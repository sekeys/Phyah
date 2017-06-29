using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Entity
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime PublishDate { get; set; }
        public string Publisher { get; set; }
        public string Content { get; set; }
        public string Quotes { get; set; }
        public string Author { get; set; }
    }
}
