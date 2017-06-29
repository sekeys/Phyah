using Phyah.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Entity
{
    public class Menu : IEntity
    {
        public string Id { get ; set ; }
        public string Name { get; set; }
        public string Href { get; set; }
        public string Parent { get; set; }
        public bool? Disabled { get; set; }
        public int Sort { get; set; }
        public string System { get; set; }
    }
}
