﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Models
{

    [Table("page_modules")]
    public class Modules : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Html { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
