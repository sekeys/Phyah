

namespace Phyah.Web
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;
    using Phyah.Interface;
    using Microsoft.Extensions.Primitives;
    using Phyah.Configuration;
    using Phyah.Concurrency;

    public class PhyahMiddleware
    {

        private RequestDelegate _next;
        public PhyahMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await Task.Run(() =>
            {
                try
                {
                    AccessorContext.DefaultContext.Set<HttpContext>(context);
                    Initialization.Pipeline.Start();
                    Initialization.Pipeline.Wait();
                }
                catch (Exception ex)
                {
                    context.Response.WriteAsync(ex.ToString());
                }
            });
           
            //System.Threading.Thread.Sleep(5000);
        }
    }
}
