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
    public class CardsBehavior : RestBehavior
    {
        public override string Text => "cards";
        readonly ICardService Service;
        public CardsBehavior()
        {
            Service = new CardService();
        }
        public async Task Get()
        {
            try
            {
                var current = Request.Query["current"].ToString().ToInt32(1);
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                var item = (await Service.MultipleAsync(m => m.System == Host)).Select(m => new {
                    id = m.Id,
                    name = m.Name,
                    disabled = m.Disabled,
                    no = m.CardNo,
                    classify = m.Classify,
                    data = m.Data,

                    remark = m.Remark,
                    sort = m.Sort,
                    description = m.Description,
                    href = m.Href,
                    source = m.ImageSource,
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
                Service.New(new Entity.Card()
                {
                    Id = Guid.NewGuid().ToString(),
                    CardNo = Request.Form["CardNo"],
                    Classify = Request.Form["Classify"],
                    Data = Request.Form["Data"],
                    Description = Request.Form["Description"],
                    Href = Request.Form["Href"],
                    ImageSource = Request.Form["ImageSource"],
                    Name = Request.Form["Name"],
                    Title = Request.Form["title"],
                    Remark = Request.Form["Remark"],
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
                var entity = (from item in Service.Context.Set<Card>()
                              where item.Id == Request.Query["id"].ToString()
                              select item).Select(m => new {
                                  id = m.Id,
                                  name = m.Name,
                                  disabled = m.Disabled,
                                  no = m.CardNo,
                                  classify = m.Classify,
                                  data = m.Data,

                                  remark = m.Remark,
                                  sort = m.Sort,
                                  description = m.Description,
                                  href = m.Href,
                                  source = m.ImageSource,
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
                Service.Update(new Card()
                {
                    Id = Guid.NewGuid().ToString(),
                    CardNo = Request.Form["CardNo"],
                    Classify = Request.Form["Classify"],
                    Data = Request.Form["Data"],
                    Description = Request.Form["Description"],
                    Href = Request.Form["Href"],
                    ImageSource = Request.Form["ImageSource"],
                    Name = Request.Form["Name"],
                    Title = Request.Form["title"],
                    Remark = Request.Form["Remark"],
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
