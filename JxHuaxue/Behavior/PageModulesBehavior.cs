using Phyah.Huaxue.Biz;
using Phyah.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue
{

    public class PageModulesBehavior : RestBehavior
    {
        readonly PageService Service;
        readonly PageModulesService PageModuleService;
        public PageModulesBehavior()
        {
            Service = new PageService();
            PageModuleService = new PageModulesService();
        }
        public override string Text => "pagemodules";

        public async Task Get()
        {
            try
            {
                var id = Request.Query["id"].ToString();
                //Tuple<int, IList<Card>> item = await Service.PaginationAsync(null, current);
                var item = Service.Modules(id);
                await Json( item );
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Post()
        {
            try
            {
                PageModuleService.Add(new Models.PageModules()
                {
                    Id = Guid.NewGuid().ToString(),
                    Index = 1,
                    Children = Request.Form["moduleid"],
                    PageId = Request.Form["pageid"]
                });
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Head()
        {
            try
            {
                string id = Request.Query["id"].ToString();
                var page = Service.Single(id);
                if (page != null)
                {

                }
                await Json(new { result = true, page = page, modules = Service.Modules(id) });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Delete()
        {
            try
            {
                string id = Request.Form["id"].ToString();
                PageModuleService.Delete(id);
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task PUT()
        {
            try
            {
                PageModuleService.Add(new Models.PageModules()
                {
                    Id = Request.Form["Id"],
                    Index = 1,
                    Children = Request.Form["moduleid"],
                    PageId = Request.Form["pageid"]
                });
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
    }
}
