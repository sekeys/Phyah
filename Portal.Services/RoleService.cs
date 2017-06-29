
namespace Portal.Services
{
    using Portal.Entity;
    using System;
    using Phyah.EntityFramework.Services;
    using Microsoft.EntityFrameworkCore;
    using Portal.IServices;

    public class RoleService : Service<Role>, IRoleService
    {
        public RoleService(DbContext Context) : base(Context)
        {
        }
        public RoleService( ) : base(new DataContext())
        {
        }
    }
}
