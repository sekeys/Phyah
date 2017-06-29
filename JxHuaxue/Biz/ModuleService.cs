using Phyah.Huaxue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Biz
{
    public class ModuleService : ServiceBase<Modules>
    {
        public ModuleService():base(new DataContext())
        {

        }
        
    }
}
