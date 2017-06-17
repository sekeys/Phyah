

namespace Phyah.Web.Handler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Phyah.Concurrency;
    using Microsoft.AspNetCore.Http;
    using Phyah.Interface;
    using Microsoft.Extensions.Primitives;
    using Phyah.Configuration;

    public class InitializedHandler : WebHandler
    {
        protected override  void HandleCore()
        {
            
            //string requestPath = HttpContext.Request.Path.ToUriComponent().Trim(new char[] { '/', '\\' });
            //if (string.IsNullOrWhiteSpace(requestPath))
            //{
            //    requestPath = AppSetting.AppSettings["defaultweb"]?.ToString();
            //    if (string.IsNullOrWhiteSpace(requestPath))
            //    {
            //        requestPath = "Index";
            //    }
            //}
            //AccessorContext.DefaultContext.Set<IPath>(Web.Path.Parse(requestPath));

            //var user = await SecurityFormTicks.Get(HttpContext);
            //Logs.Log.Degug();
            //AccessorContext.DefaultContext.Set<IUser>(user);
        }
    }
}