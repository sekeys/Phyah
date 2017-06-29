

namespace Phyah.Huaxue
{
    using Microsoft.AspNetCore.Http;
    using Phyah.Configuration;
    using Phyah.Huaxue.Biz;
    using Phyah.Huaxue.Models;
    using Phyah.Web;
    using Phyah.Web.Razor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class IndexBehavior : Behavior
    {
        public override string Text => "Index";

        public override Task Invoke()
        {

            string content = System.IO.File.ReadAllText($@"{AppSetting.AppSettings["hostdir"]}\index.html");
            return HttpContext.Response.WriteAsync(content);

            //HttpContext.Response.
        }
    }
    public class RazorBehavior : Behavior
    {
        public class IndexViewModel
        {
            public List<Card> IndexSecond { get; set; }
        }
        public override string Text => "razor";

        public override Task Invoke()
        {
            dynamic viewBag = new DynamicDictionary();
            var cardService = new CardService();
            viewBag.TopBanner = cardService.Single(m => m.CardNo == "Index_Top_Banner");
            var IndexSecond = cardService.FindAll(m => m.CardNo == "Index_Second");
            viewBag.IndexSecond = IndexSecond;
            viewBag.IndexThird= cardService.FindAll(m => m.CardNo == "Index_Third");
            viewBag.IndexFour = cardService.FindAll(m => m.CardNo == "Index_Four");
            viewBag.IndexFive = cardService.FindAll(m => m.CardNo == "Index_Five");
            string content = new RazorViewEngine().RenderView(HttpContext, @"index", null, viewBag);
            return HttpContext.Response.WriteAsync(content);
            //HttpContext.Response.
        }
    }
    public class UserListBehavior : Behavior
    {
        public override string Text => "userlist";

        public async override Task Invoke()
        {
            //await HttpContext.Response.WriteAsync(System.IO.File.ReadAllText(@"E:\DevSource\Phyah\Phyah.Huaxue\index.html"));
            await HtmlFile(@"E:\DevSource\Phyah\Phyah.Huaxue\Behind\UserList.html");
            //HttpContext.Response.
        }
    }
    //public class CardBehavior : Behavior
    //{
    //    public override string Text => "card";

    //    public async override Task Invoke()
    //    {
    //        //await HttpContext.Response.WriteAsync(System.IO.File.ReadAllText(@"E:\DevSource\Phyah\Phyah.Huaxue\index.html"));
    //        await HtmlFile(@"E:\DevSource\Phyah\Phyah.Huaxue\Behind\card.html");
    //        //HttpContext.Response.
    //    }
    //}
}
