using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Web.Razor
{
    public interface IViewEngine
    {
        string ExtensionName { get; }
        string RenderView(HttpContext context, string viewName, object model, DynamicDictionary viewbag);

        Task<string> RenderViewAsync(HttpContext context, string viewName, object model, DynamicDictionary viewbag);

    }
}
