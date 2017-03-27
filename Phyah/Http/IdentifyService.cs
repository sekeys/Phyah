

namespace Phyah.Http
{
    using Microsoft.AspNetCore.Http;
    using Phyah.Container;
    using Phyah.Identity;
    using System;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;


    public class IdentityService : IIdetntityService
    {
        public bool Enabled { get; set; } = true;
        public string Schema { get; set; } = "u.who";
        public async void SignIn(HttpContext context, IUser user)
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
        public async void SignIn(HttpContext context, IUser user, Microsoft.AspNetCore.Http.Authentication.AuthenticationProperties authenticationProperties)
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

        public void SignIn(HttpContext context, IUser who, int expirDay)
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
        public async void SignOut(HttpContext context)
        {
            await context.Authentication.SignOutAsync(Schema);
        }
        public async Task<IUser> Get(HttpContext context, Func<IIdentity, IUser> func)
        {
            var authenticaInfo = await context.Authentication.GetAuthenticateInfoAsync(Schema);

            if (authenticaInfo == null || authenticaInfo.Principal == null)
            {
                return null;
            }
            var iid = authenticaInfo.Principal.Identity as ClaimsIdentity;
            return func.Invoke(iid);
        }
        public static IIdetntityService Service
        {
            get
            {
                return Constructor.Construct<IIdetntityService>() ?? new IdentityService();
            }
        }
    }
}
