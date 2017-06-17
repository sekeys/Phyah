using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Web.Razor
{
    public class RazorViewEngine : IViewEngine
    {

        private ITemplateService templateService = new DefaultTemplateService();

        public RenderTemplateContext RenderContext { private set; get; }

        public string ExtensionName { get { return ".cshtml"; } }

        public string RenderView(HttpContext httpContext, string viewName, object model, DynamicDictionary viewbag)
        {
            using (var context = new RenderTemplateContext()
            {
                TemplateName = viewName,
                Path = httpContext.Request.Path,
                Model = model,
                ModelType = model?.GetType(),
                ViewBag = viewbag
            })
            {

                IRazorView view = templateService.GetTemplate(context);
                view.Render();
                this.RenderContext = context;
                return context.ToString();
            }
        }


        public Task<string> RenderViewAsync(HttpContext context, string viewName, object model, DynamicDictionary viewbag)
        {
            return Task.Factory.StartNew(() => RenderView(context, viewName, model, viewbag));
        }





    }
}
