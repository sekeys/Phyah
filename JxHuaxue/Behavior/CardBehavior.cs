using Phyah.Extensions;
using Phyah.Huaxue.Biz;
using Phyah.Huaxue.Models;
using Phyah.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phyah.Huaxue
{
    public class CardsBehavior : RestBehavior
    {
        readonly CardService Service;
        public override string Text => "cards";
        public CardsBehavior()
        {
            Service = new CardService();
        }
        public async Task Get()
        {
            try
            {
                var current = Request.Query["current"].ToString().ToInt32(1);
                //Tuple<int, IList<Card>> item = await Service.PaginationAsync(null, current);
                var item = Service.FindAll();
                //await Json(new
                //{
                //    count = item.Item1,
                //    data = item.Item2,
                //    current= current
                //});
                await Json(item);
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
                Service.Add(new Models.Card()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Request.Form["name"],
                    CardNo = Request.Form["cardNo"],
                    Description = Request.Form["description"],
                    Href = Request.Form["href"],
                    ImageSource = Request.Form["imagesource"],
                    Data = Request.Form["data"],
                    Remark = Request.Form["remark"],
                    Title = Request.Form["title"]
                });
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task Head()
        {
            try
            {
                await Json((from item in Service.Context.Set<CardInfoes>()
                            where item.Id == Request.Query["id"]
                            select item).FirstOrDefault());
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
                Service.Update(new Models.Card()
                {
                    Id = Request.Form["id"],
                    Name = Request.Form["name"],
                    CardNo = Request.Form["cardNo"],
                    Description = Request.Form["description"],
                    Href = Request.Form["href"],
                    ImageSource = Request.Form["imagesource"],
                    Data = Request.Form["data"],
                    Remark = Request.Form["remark"],
                    Title = Request.Form["title"]
                });
                await Json(new { result=true});
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
    }
}
