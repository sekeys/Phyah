using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;
using Phyah.Concurrency;

namespace Phyah.Web.Router
{
    public class RouterManager
    {
        public static IRouter DefaultRouter { get; set; } = new DefaultRouter();
        /// <summary>
        /// 匹配路径，获取路由
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IRouter Routing(IPath path)
        {
            foreach (var item in Routers.Values)
            {
                if (item.Match(path))
                {
                    return item;
                }
            }
            return DefaultRouter.Match(path) ? DefaultRouter : null;
        }
        private Dictionary<string, IRouter> Routers { get; set; } = new Dictionary<string, IRouter>();

        public RouterManager() { }

        public void Register(string name, IRouter router)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = nameof(router);
            }
            if (Routers.ContainsKey(name))
            {
                throw new Exception("存在相同的路由");
            }
            Routers.Add(name, router);
        }
        public void Register(IRouter router) => Register("", router);

        public void Unregister(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { name = nameof(name); }
            if (!Routers.ContainsKey(name)) { return; }
            Routers.Remove(name);
        }
        public void Unregister(IRouter router) => Unregister(nameof(router));

        private static RouterManager _Manager;
        private static object Locker = new object();
        public static RouterManager Manager
        {
            get
            {
                if (_Manager == null)
                {
                    lock (Locker)
                    {
                        if (_Manager == null)
                            _Manager = new RouterManager();
                    }
                }
                return _Manager;
            }
        }
    }
}
