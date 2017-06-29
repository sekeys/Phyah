using Phyah.EntityFramework.Services;
using Portal.Entity;
using Portal.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Services
{
    public class MenuService : Service<Menu>, IMenuService
    {
        public MenuService(DbContext Context) : base(Context)
        {
        }

        public MenuService() : base(new DataContext())
        {
        }

        public IEnumerable<Menu> MenusInRole(IEnumerable<Role> roles)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Menu>> MenusInRoleAsync(IEnumerable<string> roles, string userid, string system)
        {
            roles = roles.Append(userid);
            var self = from menu in Set().Where(m => !string.IsNullOrWhiteSpace(m.Parent) && (!m.Disabled == null || !m.Disabled.Value))
                       join m in Set().Where(m => (!m.Disabled == null || !m.Disabled.Value))
                       on menu.Parent equals m.Id
                       select new
                       {
                           Name = menu.Name,
                           Id = menu.Id,
                           Href = menu.Href,
                           Disabledm = menu.Disabled,
                           Parent = menu.Parent,
                           Sort = menu.Sort,
                           System = menu.System,
                           ParentName = m.Name
                       };
            var roleMenus = Context.Set<Rel_Menus>().Where(m => system.Equals(m.System));
            var q1 =
                from menu in Set()
                from rm in roleMenus
                where roles.Contains(rm.Verdor)
                join m in Set()
                on rm.MenuId equals m.Id
                orderby m.Sort
                select m;

            var q2 =
                from menu in Set()
                from rm in roleMenus
                where roles.Contains(rm.Verdor)
                join m in Set()
                on rm.MenuId equals m.Parent
                orderby m.Sort
                select m;

            q1 = q1.Union(q2);

            return await q1.Distinct().ToListAsync();
        }
        public IEnumerable<Menu> MenusInRole(IEnumerable<string> roles, string userid,string system)
        {
            roles = roles.Append(userid);
            var self = from menu in Set().Where(m => !string.IsNullOrWhiteSpace(m.Parent) && (!m.Disabled == null || !m.Disabled.Value))
                       join m in Set().Where(m => (!m.Disabled == null || !m.Disabled.Value))
                       on menu.Parent equals m.Id
                       select new
                       {
                           Name = menu.Name,
                           Id = menu.Id,
                           Href = menu.Href,
                           Disabledm = menu.Disabled,
                           Parent = menu.Parent,
                           Sort = menu.Sort,
                           System = menu.System,
                           ParentName = m.Name
                       };
            var roleMenus = Context.Set<Rel_Menus>().Where(m => system.Equals(m.System));
            var q1 =
                from menu in Set()
                from rm in roleMenus
                where roles.Contains(rm.Verdor)
                join m in Set()
                on rm.MenuId equals m.Id
                orderby m.Sort
                select m;

            var q2 =
                from menu in Set()
                from rm in roleMenus
                where roles.Contains(rm.Verdor)
                join m in Set()
                on rm.MenuId equals m.Parent
                orderby m.Sort
                select m;

            q1 = q1.Union(q2);

            return q1.Distinct().ToList();
        }
        public async Task<IEnumerable<Menu>> MenusInRoleAsync(IEnumerable<string> roles, string userid)
        {
            roles = roles.Append(userid);
            var self = from menu in Set().Where(m => !string.IsNullOrWhiteSpace(m.Parent) && (!m.Disabled==null || !m.Disabled.Value))
                       join m in Set().Where(m => (!m.Disabled == null || !m.Disabled.Value))
                       on menu.Parent equals m.Id
                       select new
                       {
                           Name = menu.Name,
                           Id = menu.Id,
                           Href = menu.Href,
                           Disabledm = menu.Disabled,
                           Parent = menu.Parent,
                           Sort = menu.Sort,
                           System = menu.System,
                           ParentName = m.Name
                       };
            /*
             * 1  b 2
             * 2  c 3
             * 3  d x
             * */
            /*
            * 1  b 2 c
            * 2  c 3 d
            * --3  d x  
            * */
            var q1 =
                from menu in Set()
                from rm in Context.Set<Rel_Menus>()
                where roles.Contains(rm.Verdor)
                join m in Set()
                on rm.MenuId equals m.Id
                orderby m.Sort
                select m;

            var q2 =
                from menu in Set()
                from rm in Context.Set<Rel_Menus>()
                where roles.Contains(rm.Verdor)
                join m in Set()
                on rm.MenuId equals m.Parent
                orderby m.Sort
                select m;

            q1 = q1.Union(q2);

            return await q1.Distinct().ToListAsync();
        }
        public IEnumerable<Menu> MenusInRole(IEnumerable<string> roles, string userid)
        {
            roles=roles.Append(userid);
            var self = from menu in Set().Where(m => !string.IsNullOrWhiteSpace(m.Parent) && (!m.Disabled == null || !m.Disabled.Value))
                       join m in Set().Where(m => (!m.Disabled == null || !m.Disabled.Value))
                       on menu.Parent equals m.Id
                       select new
                       {
                           Name=menu.Name,
                           Id=menu.Id,
                           Href=menu.Href,
                           Disabledm = menu.Disabled,
                           Parent = menu.Parent,
                           Sort = menu.Sort,
                           System = menu.System,
                           ParentName=m.Name
                       };
            /*
             * 1  b 2
             * 2  c 3
             * 3  d x
             * */
            /*
            * 1  b 2 c
            * 2  c 3 d
            * --3  d x  
            * */
            var q1 =
                from menu in Set()
                from rm in Context.Set<Rel_Menus>()
                where roles.Contains(rm.Verdor)
                join m in Set()
                on rm.MenuId equals m.Id
                orderby m.Sort
                select m;

            var q2 =
                from menu in Set()
                from rm in Context.Set<Rel_Menus>()
                where roles.Contains(rm.Verdor)
                join m in Set()
                on rm.MenuId equals m.Parent
                orderby m.Sort
                select m;

            q1 = q1.Union(q2);

            return q1.Distinct().ToList();
        }

        public void NewRel(string menuid, string system, string vendor) => NewRel(new Rel_Menus()
        {
            Id = Guid.NewGuid().ToString(),
            MenuId = menuid,
            System = system,
            Verdor = vendor,
        });
        public void NewRel(Rel_Menus entity)
        {
            new Service<Rel_Menus>(this.Context).NewOrUpdate(entity);
        }

        public void ReplaceRel(string id, string menuid, string system, string vendor)=> ReplaceRel(new Rel_Menus()
        {
            Id = Guid.NewGuid().ToString(),
            MenuId = menuid,
            System = system,
            Verdor = vendor,
        });

        public void ReplaceRel(Rel_Menus entity)
        {
            new Service<Rel_Menus>(this.Context).Update(entity);
        }

        public void RemoveRel(string id)
        {
            new Service<Rel_Menus>(this.Context).Delete(id);
        }

        public async Task NewRelAsync(string menuid, string system, string vendor)=>await NewRelAsync(new Rel_Menus()
        {
            Id = Guid.NewGuid().ToString(),
            MenuId = menuid,
            System = system,
            Verdor = vendor,
        });

        public async Task NewRelAsync(Rel_Menus entity)
        {
           await new Service<Rel_Menus>(this.Context).NewOrUpdateAsync(entity);
        }

        public async Task ReplaceRelAsync(string id, string menuid, string system, string vendor)=> await ReplaceRelAsync(new Rel_Menus()
        {
            Id = Guid.NewGuid().ToString(),
            MenuId = menuid,
            System = system,
            Verdor = vendor,
        });
        public async Task ReplaceRelAsync(Rel_Menus entity)
        {
            await new Service<Rel_Menus>(this.Context).UpdateAsync(entity);
        }

        public async Task<int> RemoveRelAsync(string id)
        {
            return await new Service<Rel_Menus>(this.Context).DeleteAsync(id);
        }

        public void Disabled(string id) => Update(new Menu() { Id = id, Disabled = true });

        public void Enabled(string id) => Update(new Menu() { Id = id, Disabled = false });

        public async Task DisabledAsync(string id)=> await UpdateAsync(new Menu() { Id = id, Disabled = true });

        public async Task EnabledAsync(string id) => await UpdateAsync(new Menu() { Id = id, Disabled = false });
    }
}
