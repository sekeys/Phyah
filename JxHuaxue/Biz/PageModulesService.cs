using Microsoft.EntityFrameworkCore;
using Phyah.Huaxue.Biz;
using Phyah.Huaxue.Models;

namespace Phyah.Huaxue
{
    public class PageModulesService : ServiceBase<PageModules>
    {
        public PageModulesService() : base(new DataContext())
        {
        }
    }
}