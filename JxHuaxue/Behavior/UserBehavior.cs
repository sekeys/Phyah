using Phyah.Huaxue.Biz;
using Phyah.Huaxue.Models;
using Phyah.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue
{
    public class UserBehavior : RestBehavior
    {
        readonly UsersService Service;
        public override string Text => "Users";
        public UserBehavior()
        {
            Service = new UsersService();
        }
        public async Task Get()
        {
            try
            {
                await Json(Service.FindAll().Select(m => new { Id = m.Id, UserName = m.UserName, Nick = m.Nick }));
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
                Service.Add(new Models.Users()
                {
                    Id = Guid.NewGuid().ToString(),
                    Nick = Request.Form["nick"],
                    Password = Request.Form["pwd"],
                    UserName = Request.Form["account"]
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
                await Json((from item in Service.Context.Set<Users>()
                            where item.Id == Request.Query["id"]
                            select item).FirstOrDefault());
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
                Service.ResetPwd(new Models.Users()
                {
                    Id = Request.Form["id"],
                    Password = Request.Form["pwd"]
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
