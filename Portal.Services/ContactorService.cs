
namespace Portal.Services
{
    using Portal.Entity;
    using System;
    using Phyah.EntityFramework.Services;
    using Microsoft.EntityFrameworkCore;
    using Portal.IServices;

    public class ContactorService : Service<Contactor>, IContactorService
    {
        public ContactorService(DbContext Context) : base(Context)
        {
        }
        public ContactorService( ) : base(new DataContext())
        {
        }
    }
}
