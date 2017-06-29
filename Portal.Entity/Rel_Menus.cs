using Phyah.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portal.Entity
{
    public class Rel_Menus:IEntity
    {
        public string Id { get; set; }
        public string MenuId { get; set; }
        public string Verdor { get; set; }
        public string System { get; set; }

    }
}
