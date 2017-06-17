using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;
using Microsoft.AspNetCore.Http;

namespace Phyah.Web.Router
{
    public class DefaultRouter : IRouter
    {
        public int Priority => 100;
        public bool Match(IPath path)
        {
            return true;
        }

        public IBehavior Next(IPath path)
        {
            if (path.Next())
            {
                return BehaviorFactory.Factory.Cache(path.Current.Raw);
            }
            return null;
        }

        //public bool Routing(string pattern, out IMiddleware middleware)
        //{
        //    middleware = Middleware.MiddlewareFactory.Factory.Cache(pattern);
        //    return true;
        //}
    }
}
