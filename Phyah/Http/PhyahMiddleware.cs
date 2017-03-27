

namespace Phyah.Http
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class PhyahMiddleware
    {
        private readonly RequestDelegate _next;
        public PhyahMiddleware(RequestDelegate next)
        {
            _next = next;
        }
    }
}
