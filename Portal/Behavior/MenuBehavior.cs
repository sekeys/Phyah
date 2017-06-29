using Phyah;
using Phyah.Concurrency;
using Phyah.Extensions;
using Phyah.Interface;
using Phyah.Web;
using Phyah.Web.Attributes;
using Portal.Entity;
using Portal.IServices;
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Behavior
{
    [RouterOnMethod("PathRoute")]
    public class MenuBehavior : RestBehavior
    {
        public override string Text => "menus";
        readonly IMenuService Service;
        public MenuBehavior()
        {
            Service = new MenuService();
        }
        public string PathRoute(IPath path)
        {
            path.Next();
            return path.Current.Raw;
        }
        public async Task Get()
        {
            try
            {
                var current = Request.Query["current"].ToString().ToInt32(1);
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                var item = (await Service.MultipleAsync(m => m.System == Host)).Select(m => new {
                    id = m.Id,
                    name = m.Name,
                    disabled = m.Disabled,
                    href = m.Href,
                    parent = m.Parent,
                    sort = m.Sort,
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
                Service.New(new Entity.Menu()
                {
                    Id = Guid.NewGuid().ToString(),
                     Parent = Request.Form["parent"],
                    Href = Request.Form["Href"],
                    Name = Request.Form["Name"],
                    System = Host,
                    Disabled = Request.Form["disabled"].ToString()?.ToBoolean(),
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
                var entity = (from item in Service.Context.Set<Menu>()
                              where item.Id == Request.Query["id"].ToString()
                              select item).Select(m => new {
                                  id = m.Id,
                                  name = m.Name,
                                  disabled = m.Disabled,
                                  href = m.Href,
                                  parent = m.Parent,
                                  sort = m.Sort,
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
                Service.Update(new Entity.Menu()
                {
                    Id = Guid.NewGuid().ToString(),
                    Parent = Request.Form["parent"],
                    Href = Request.Form["Href"],
                    Name = Request.Form["Name"],
                    System = Host,
                    Disabled = Request.Form["disabled"].ToString()?.ToBoolean(),
                    Sort = Request.Form["sort"].ToString().ToInt32(0)
                });
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Enable()
        {
            try
            {
                await Service.EnabledAsync(Request.Form["id"]);
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Disabled()
        {
            try
            {
                await Service.DisabledAsync(Request.Form["id"]);
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Remove()
        {
            try
            {
                await Service.RemoveRelAsync(Request.Form["id"]);
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Rel()
        {
            try
            {
                string host = AccessorContext.DefaultContext.Get<string>("host");
                await Service.NewRelAsync(Request.Form["m"],host,Request.Form["v"]);
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task UpdateRel()
        {
            try
            {
                string host = AccessorContext.DefaultContext.Get<string>("host");
                await Service.ReplaceRelAsync(Request.Form["id"],Request.Form["m"], host, Request.Form["v"]);
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
    }
}
