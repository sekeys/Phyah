

namespace Portal.Behavior
{
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
    public class ContactorBehavior : RestBehavior
    {
        public override string Text => "contactor";
        readonly IContactorService Service;
        public ContactorBehavior()
        {
            Service = new ContactorService();
        }
        public async Task Get()
        {
            try
            {
                var current = Request.Query["current"].ToString().ToInt32(1);
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                var item = (await Service.MultipleAsync(m => m.System == Host)).Select(m => new
                {
                    id = m.Id,
                    name = m.Name,
                    disabled = m.Disabled,
                    form = m.FormCode,
                    information = m.Information,
                    phone = m.Phone,
                    remark = m.Remark,
                    sort = m.Sort,
                    submitdate = m.SubtmitDate,
                }); ;
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
                Service.New(new Contactor()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Request.Form["name"],
                    Phone = Request.Form["Phone"],
                    Remark = Request.Form["remark"],
                    FormCode = Request.Form["FormCode"],
                    Information = Request.Form["Information"],
                    SubtmitDate = DateTime.Now,
                    System = Host,
                    Disabled = false,
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
                var entity = (from item in Service.Context.Set<Contactor>()
                              where item.Id == Request.Form["id"].ToString()
                              select item).Select(m => new
                              {
                                  id = m.Id,
                                  name = m.Name,
                                  disabled = m.Disabled,
                                  form = m.FormCode,
                                  information = m.Information,
                                  phone = m.Phone,
                                  remark = m.Remark,
                                  sort = m.Sort,
                                  submitdate = m.SubtmitDate,
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
                await Service.UpdateAsync(Request.Form["id"], new Contactor()
                {
                    Id = Request.Form["id"],
                    Name = Request.Form["name"],
                    Phone = Request.Form["Phone"],
                    Remark = Request.Form["remark"],
                    FormCode = Request.Form["FormCode"],
                    Information = Request.Form["Information"],
                    SubtmitDate = DateTime.Now,
                    System = Host,
                    Disabled = false,
                    Sort = Request.Form["sort"].ToString().ToInt32(0)
                });
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
        public async Task DELETE()
        {
            try
            {
                string Host = AccessorContext.DefaultContext.Get<string>("host");
                await Service.DeleteAsync(Request.Query["id"]);
                await Json(new { result = true });
            }
            catch (Exception ex)
            {
                await Status(StatusCode.UNKNOWERROR, ex.Message);
            }
        }
    }
}
