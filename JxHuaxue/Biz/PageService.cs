using Phyah.Huaxue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Biz
{
    public class PageService : ServiceBase<Pages>
    {
        public PageService() : base(new DataContext())
        {

        }

        public string PageContent(string pageid)
        {
            var result = (from p in this.Context.Set<Pages>()
                          join pl in this.Context.Set<PageModules>()
                          on p.Id equals pl.PageId
                          join m in this.Context.Set<Modules>()
                          on pl.Children equals m.Id
                          where p.Id == pageid
                          orderby pl.Index descending
                          select m.Html).ToList();
            if (result == null)
            {
                return "";
            }
            string page = "";
            foreach (var item in result)
            {
                page += $"\r\n{item}";
            }
            return page;
        }
        public List<PageModuleEntity> Modules(string pageid)
        {
            var result = from r in this.Context.Set<PageModules>()
                         join m in this.Context.Set<Modules>()
                         on r.Children equals m.Id
                         where r.PageId == pageid
                         orderby r.Index descending
                         select new PageModuleEntity
                         {
                             CreateTime = m.CreateTime,
                             Description = m.Description,
                             Html = m.Html,
                             PageId=r.PageId,
                             Id = r.Id,
                             ModuleId=m.Id,
                             Index = r.Index,
                             Name = m.Name
                         };
            return result.ToList();
        }
    }
}
