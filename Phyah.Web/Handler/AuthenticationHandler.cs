

namespace Phyah.Web.Handler
{
    using Microsoft.AspNetCore.Http;
    using Phyah.Interface;
    using Phyah.Web.Exceptions;
    using System;
    public class AuthenticationHandler : WebHandler
    {
        readonly Func<HttpContext, IUser, bool> OnAuthentication;
        public AuthenticationHandler(Func<HttpContext, IUser, bool> authentication)
        {
            OnAuthentication = authentication;
        }
        protected override void HandleCore()
        {
            if (!OnAuthentication(HttpContext, User))
            {
                throw new AuthenticationFailedException("无法访问资源，或者该资源不存在");
            }

        }
    }
}
