using Phyah.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Entity
{
    public class BaseEntity:IEntity
    {
        public string Id { get; set; }
        public string System { get; set; }
        public int Sort { get; set; }
        public bool? Disabled { get; set; }
    }
}
