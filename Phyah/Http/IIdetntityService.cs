using Microsoft.AspNetCore.Http;
using Phyah.Identity;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Http
{
    public interface IIdetntityService
    {
        void SignIn(HttpContext context, IUser user);
        void SignIn(HttpContext context, IUser user, Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties authenticationProperties);

        void SignIn(HttpContext context, IUser who, int expirDay);
        void SignOut(HttpContext context);
        Task<IUser> Get(HttpContext context, Func<IIdentity, IUser> func);
    }
}
