using Phyah.Huaxue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue.Biz
{
    public class UsersService:ServiceBase<Users>
    {
        public UsersService():base(new DataContext())
        {

        }
        public void ResetPwd(Users users)
        {
            var model = (from u in this.Context.Set<Users>()
                        where u.Id == users.Id
                        select u).FirstOrDefault() ;
            if (model == null)
            {
                return;
            }
            model.Password = users.Password;
            Update(model);
        }
    }
}
