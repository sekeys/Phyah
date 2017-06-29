using Phyah;
using Phyah.Concurrency;
using Phyah.Extensions;
using Phyah.Web;
using Portal.Entity;
using Portal.IServices;
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Behavior
{
    public class ArticleBehavior : RestBehavior
    {
        public override string Text => "Article";
        readonly IArticleService Service;
        public ArticleBehavior()
        {
            Service = new ArticleService();
        }
        public async Task Get()
        {
            try
            {
                var current = Request.Query["current"].ToString().ToInt32(1);
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                var item = (await Service.MultipleAsync(m => m.System == Host)).Select(m => new {
                    id = m.Id,
                    author = m.Author,
                    disabled = m.Disabled,
                    content = m.Content,
                    publishdate = m.PublishDate,
                    publisher = m.Publisher,
                    quotes = m.Quotes,
                    sort = m.Sort,
                    subtitle = m.Subtitle,
                    title = m.Title
                });
                await Json(new
                {
                    result = true,
                    //count = item.Item1,
                    //data = item.Item2,
                    current = current,
                    data = item
                });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Post()
        {
            try
            {
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                Service.New(new Entity.Article()
                {
                    Id = Guid.NewGuid().ToString(),
                    Author = Request.Form["Author"],
                    Content = Request.Form["Content"],
                    PublishDate = Request.Form["PublishDate"].ToString().ToDateTime(DateTime.Now),
                    Publisher = Request.Form["Publisher"],
                    Quotes = Request.Form["Quotes"],
                    Subtitle = Request.Form["Subtitle"],
                    Title = Request.Form["title"],
                    System = Host,
                    Disabled = Request.Form["disabled"].ToString().ToBoolean(),
                    Sort = Request.Form["sort"].ToString().ToInt32(0)
                });
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task PATCH()
        {
            try
            {
                var entity = (from item in Service.Context.Set<Article>()
                              where item.Id == Request.Query["id"].ToString()
                              select item).Select(m => new
                              {
                                  id = m.Id,
                                  author = m.Author,
                                  disabled = m.Disabled,
                                  content = m.Content,
                                  publishdate = m.PublishDate,
                                  publisher = m.Publisher,
                                  quotes = m.Quotes,
                                  sort = m.Sort,
                                  subtitle = m.Subtitle,
                                  title = m.Title
                              }).FirstOrDefault();
                await Json(new
                {
                    result = true,
                    data = entity
                });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task PUT()
        {
            try
            {
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                Service.Update(new Article()
                {
                    Id = Request.Form["id"],
                    Author = Request.Form["Author"],
                    Content = Request.Form["Content"],
                    PublishDate = Request.Form["PublishDate"].ToString().ToDateTime(DateTime.Now),
                    Publisher = Request.Form["Publisher"],
                    Quotes = Request.Form["Quotes"],
                    Subtitle = Request.Form["Subtitle"],
                    Title = Request.Form["title"],
                    System = Host,
                    Disabled = Request.Form["disabled"].ToString().ToBoolean(),
                    Sort = Request.Form["sort"].ToString().ToInt32(0)
                });
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
    }
}
