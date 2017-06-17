using Microsoft.AspNetCore.Http;
using Phyah.Concurrency;
using Phyah.Configuration;
using Phyah.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web.Handler
{
    public class PathResetHandler : WebHandler
    {
        protected override void HandleCore()
        {

            string requestPath = HttpContext.Request.Path.ToUriComponent().Trim(new char[] { '/', '\\' });
            if (string.IsNullOrWhiteSpace(requestPath))
            {
                requestPath = AppSetting.AppSettings["defaultweb"]?.ToString();
                if (string.IsNullOrWhiteSpace(requestPath))
                {
                    requestPath = "Index";
                }
            }
            var path = Web.Path.Parse(requestPath);
            AccessorContext.DefaultContext.Set<IPath>(PathMappingServing.Instance.Mapping(path));
            
        }
    }
}
