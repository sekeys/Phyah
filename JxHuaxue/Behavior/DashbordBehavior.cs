using Microsoft.AspNetCore.Http;
using Phyah.Configuration;
using Phyah.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue
{
    public class DashbordBehavior : Behavior
    {
        public override string Text => "dashbord";

        public async override Task Invoke()
        {
            

            string content = System.IO.File.ReadAllText($@"{AppSetting.AppSettings["hostdir"]}\Behind\dashbord.html");
            await HttpContext.Response.WriteAsync(content);
            //HttpContext.Response.
        }
    }
    public class CardListBehavior : Behavior
    {
        public override string Text => "cardlist";

        public async override Task Invoke()
        {


            string content = System.IO.File.ReadAllText($@"{AppSetting.AppSettings["hostdir"]}\Behind\cardlist.html");
            await HttpContext.Response.WriteAsync(content);
            //HttpContext.Response.
        }
    }
    public class ModuleListBehavior : Behavior
    {
        public override string Text => "modulelist";

        public async override Task Invoke()
        {


            string content = System.IO.File.ReadAllText($@"{AppSetting.AppSettings["hostdir"]}\Behind\modulelist.html");
            await HttpContext.Response.WriteAsync(content);
            //HttpContext.Response.
        }
    }

    public class PageListBehavior : Behavior
    {
        public override string Text => "pagelist";

        public async override Task Invoke()
        {


            string content = System.IO.File.ReadAllText($@"{AppSetting.AppSettings["hostdir"]}\Behind\pagelist.html");
            await HttpContext.Response.WriteAsync(content);
            //HttpContext.Response.
        }
    }

}
