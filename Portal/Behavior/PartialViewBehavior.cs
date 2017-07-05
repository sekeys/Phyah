using Phyah;
using Phyah.Concurrency;
using Phyah.Extensions;
using Phyah.Web;
using Portal.Entity;
using Portal.IServices;
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Behavior
{
    public class PartialViewBehavior : RestBehavior
    {
        public override string Text => "partials";
        readonly IPartialViewService Service;
        public PartialViewBehavior()
        {
            Service = new PartialViewService();
        }
        public async Task DELETE()
        {
            try
            {
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                await Service.DeleteAsync(Request.Query["id"]);
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Get()
        {
            try
            {
                var current = Request.Query["current"].ToString().ToInt32(1);
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                var item = (await Service.MultipleAsync(m => m.System == Host)).Select(m => new {
                    id = m.Id,
                    disabled = m.Disabled,
                    content = m.Content,
                    sort = m.Sort,
                    name = m.Name,
                    created = m.CreateDate,
                    hash = m.Hash
                });
                await Json(new
                {
                    result = true,
                    //count = item.Item1,
                    //data = item.Item2,
                    current = current,
                    data = item
                });
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
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                Service.New(new Entity.PartialView()
                {
                    Id = Guid.NewGuid().ToString(),
                    Hash = Request.Form["hash"],
                    Content = Request.Form["Content"],
                    CreateDate = Request.Form["Created"].ToString().ToDateTime(DateTime.Now),
                    Name = Request.Form["name"],
                    System = Host,
                    Disabled = Request.Form["disabled"].ToString().ToBoolean(),
                    Sort = Request.Form["sort"].ToString().ToInt32(0)
                });
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task PATCH()
        {
            try
            {
                var entity = (from item in Service.Context.Set<PartialView>()
                              where item.Id == Request.Form["id"].ToString()
                              select item).Select(m => new {
                                  id = m.Id,
                                  disabled = m.Disabled,
                                  content = m.Content,
                                  sort = m.Sort,
                                  name = m.Name,
                                  created = m.CreateDate,
                                  hash = m.Hash
                              }).FirstOrDefault();
                await Json(new
                {
                    result = true,
                    data = entity
                });
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
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                await Service.UpdateAsync(Request.Form["id"], new PartialView()
                {
                    Id = Request.Form["id"],
                    Hash = Request.Form["hash"],
                    Content = Request.Form["Content"],
                    CreateDate = Request.Form["Created"].ToString().ToDateTime(DateTime.Now),
                    Name = Request.Form["name"],
                    System = Host,
                    Disabled = Request.Form["disabled"].ToString().ToBoolean(),
                    Sort = Request.Form["sort"].ToString().ToInt32(0)
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
