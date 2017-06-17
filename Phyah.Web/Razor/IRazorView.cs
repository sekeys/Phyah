using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web.Razor
{
    public interface IRazorView
    {
        RazorViewTemplate Template { get; }

        void SetContext(RenderTemplateContext context);

        void Render();

    }
}
