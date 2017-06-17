using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web.Razor
{
    public interface ITemplateService
    {

        IRazorView GetTemplate(RenderTemplateContext context);

    }
}
