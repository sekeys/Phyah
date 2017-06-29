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
    public class RoleBehavior : RestBehavior
    {
        public override string Text => "role";
        readonly IRoleService Service;
        public RoleBehavior()
        {
            Service = new RoleService();
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
                    department = m.Department,
                    sort = m.Sort,
                    description = m.Description,
                    parent = m.Parent,
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
                Service.New(new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                     Department = Request.Form["department"],
                    Description = Request.Form["Description"],
                     Parent = Request.Form["parent"],
                    Name = Request.Form["Name"],
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
                var entity = (from item in Service.Context.Set<Role>()
                              where item.Id == Request.Query["id"].ToString()
                              select item).Select(m => new {
                                  id = m.Id,
                                  name = m.Name,
                                  disabled = m.Disabled,
                                  department = m.Department,
                                  sort = m.Sort,
                                  description = m.Description,
                                  parent = m.Parent,
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
                Service.Update(new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    Department = Request.Form["department"],
                    Description = Request.Form["Description"],
                    Parent = Request.Form["parent"],
                    Name = Request.Form["Name"],
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
