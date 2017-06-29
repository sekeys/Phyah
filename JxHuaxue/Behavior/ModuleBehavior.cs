using Phyah.Extensions;
using Phyah.Huaxue.Biz;
using Phyah.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue
{
    public class ModuleBehavior : RestBehavior
    {
        readonly Biz.ModuleService Service;
        public override string Text => "modules";
        public ModuleBehavior()
        {
            Service = new ModuleService();
        }
        public async Task Get()
        {
            try
            {
                var current = Request.Query["current"].ToString().ToInt32(1);
                //Tuple<int, IList<Card>> item = await Service.PaginationAsync(null, current);
                var item = Service.FindAll();
                //await Json(new
                //{
                //    count = item.Item1,
                //    data = item.Item2,
                //    current= current
                //});
                await Json(item);
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
                Service.Add(new Models.Modules()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Request.Form["name"],
                    CreateTime = DateTime.Now,
                    Description = Request.Form["description"],
                    Html = Request.Form["html"]
                });
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
                Service.Update(new Models.Modules()
                {
                    Id = Request.Form["id"],
                    Name = Request.Form["name"],
                    Description = Request.Form["description"],
                    Html = Request.Form["html"]
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
