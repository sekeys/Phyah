using Phyah.EntityFramework.Services;
using Portal.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portal.IServices
{
    public interface IMenuService : IService<Menu>
    {
        IEnumerable<Menu> MenusInRole(IEnumerable<Role> roles);
        IEnumerable<Menu> MenusInRole(IEnumerable<string> roles, string userid);
        Task<IEnumerable<Menu>> MenusInRoleAsync(IEnumerable<string> roles, string userid);
        IEnumerable<Menu> MenusInRole(IEnumerable<string> roles, string userid, string system);
        Task<IEnumerable<Menu>> MenusInRoleAsync(IEnumerable<string> roles, string userid, string system);
        void NewRel(string menuid, string system, string vendor);
        void NewRel(Rel_Menus entity);
        void ReplaceRel(string id, string menuid, string system, string vendor);
        void ReplaceRel(Rel_Menus entity);
        void RemoveRel(string id);
        Task NewRelAsync(string menuid, string system, string vendor);
        Task NewRelAsync(Rel_Menus entity);
        Task ReplaceRelAsync(string id, string menuid, string system, string vendor);
        Task ReplaceRelAsync(Rel_Menus entity);
        Task<int> RemoveRelAsync(string id);
        void Disabled(string id);
        void Enabled(string id);
        Task DisabledAsync(string id);
        Task EnabledAsync(string id);

        /*
         * 
         * var relmenu = new Rel_Menus()
        {
            Id = "",
            MenuId = "",
            System = "",
            Verdor = "",
        }
         * */
    }
}
