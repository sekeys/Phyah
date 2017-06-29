
namespace Portal.Services
{
    using Phyah.EntityFramework.Services;
    using Portal.Entity;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;

    public class PartialViewService : Service<PartialView>, IServices.IPartialViewService
    {
        public PartialViewService(DbContext Context) : base(Context)
        {
        }
        public PartialViewService() : base(new DataContext())
        {
        }
    }
}
