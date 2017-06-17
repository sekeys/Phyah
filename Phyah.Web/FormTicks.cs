using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Phyah.Interface;

namespace Phyah.Web
{
    public class SecurityFormTicks 
    {
        public static bool Enabled { get; set; } = true;
        public static string Schema { get; set; } = "u.who";
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        public static async void SignIn(HttpContext context, IUser user)
        {
            ClaimsIdentity id = new ClaimsIdentity(user);
            foreach (var role in user.Roles)
            {
                id.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            id.AddClaim(new Claim(ClaimTypes.PrimarySid, user.Token));
            id.Label = user.Name;
            ClaimsPrincipal claimP = new ClaimsPrincipal(id);
            await context.Authentication.SignInAsync(Schema, claimP);
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="authenticationProperties"></param>
        public static async void SignIn(HttpContext context, IUser user, Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties authenticationProperties)
        {
            ClaimsIdentity id = new ClaimsIdentity(user);
            foreach (var role in user.Roles)
            {
                id.AddClaim(new Claim(ClaimTypes.Role, role));

            }

            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Token));
            //id.AddClaim(new Claim(ClaimTypes.UserData, $"{(who.Guest ? "1" : "0")}:{(who.Admin ? "1" : "0")}:{(who.SupperMan ? "1" : "0")}"));
            id.Label = user.Name;
            ClaimsPrincipal claimP = new ClaimsPrincipal(id);
            await context.Authentication.SignInAsync(Schema, claimP, authenticationProperties);
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="context"></param>
        /// <param name="who"></param>
        /// <param name="expirDay"></param>
        public static  void SignIn(HttpContext context, IUser who, int expirDay)
        {
            var curDate = DateTime.Now;
            var ExpiresUtc = new DateTimeOffset(curDate.AddDays(expirDay));
            var proper = new Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties();
            proper.IsPersistent = true;
           
            proper.ExpiresUtc = ExpiresUtc;
            proper.IssuedUtc = curDate;
            proper.AllowRefresh = true;
            proper.RedirectUri = "/login";
            SignIn(context, who, proper);
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="context"></param>
        public static async void SignOut(HttpContext context)
        {
            await context.Authentication.SignOutAsync(Schema);
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task<IUser> Get(HttpContext context)
        {
            var authenticaInfo = await context.Authentication.GetAuthenticateInfoAsync(Schema);

            if (authenticaInfo == null || authenticaInfo.Principal == null)
            {
                return null;
            }
            var iid = authenticaInfo.Principal.Identity as ClaimsIdentity;

            Claim nameIdentifer = iid.FindFirst(ClaimTypes.NameIdentifier);
            var who = new HttpUser(nameIdentifer.Value, iid.Name);
            foreach (Claim ci in iid.FindAll(ClaimTypes.Role))
            {
                who.Roles.Add(ci.Value);
            }
            Claim isadmininfo = iid.FindFirst(ClaimTypes.UserData);
            who.UserData = isadmininfo.Value;
           
            return who;
        }
        
    }
}
