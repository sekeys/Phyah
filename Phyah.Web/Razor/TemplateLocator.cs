using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Phyah.Web.Razor
{
    public class TemplateLocator
    {

        public static string LoadTemplateContent(string viewName)
        {
            string templatePath = Phyah.Web.HostingEnvironment.GetMapPath(viewName);

            //if (!File.Exists(templatePath)) throw new FileNotFoundException("view template file can't find.");
            
            string viewTemplate = null;
            if (File.Exists($"{templatePath}.cshtml"))
            {
                using (StreamReader reader = new StreamReader(new FileStream($"{templatePath}.cshtml", FileMode.Open, FileAccess.Read), Encoding.UTF8))
                {
                    viewTemplate = reader.ReadToEnd();
                }
            }
            else if (File.Exists($"{templatePath}.html"))
            {
                using (StreamReader reader = new StreamReader(new FileStream($"{templatePath}.cshtml", FileMode.Open, FileAccess.Read), Encoding.UTF8))
                {
                    viewTemplate = reader.ReadToEnd();
                }
            }
            else
            {
                throw new FileNotFoundException("view template file can't find.");
            }
            return viewTemplate;
        }



    }
}
