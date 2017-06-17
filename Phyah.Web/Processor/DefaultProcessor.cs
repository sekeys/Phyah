

namespace Phyah.Web
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Phyah.Attributes;
    using Phyah.Interface;
    //using Phyah.Debug;
    using Phyah.Web.Exceptions;
    using Phyah.Web.Router;
    using Microsoft.AspNetCore.Http;
    using Phyah.Concurrency;

    public class DefaultProcessor : Processor
    {
        private IRouter Router;
        IPath path => AccessorContext.DefaultContext.Get<IPath>();
        public  override void Process()
        {
            
            try
            {
                Router = RouterManager.Manager.Routing(null);
                if (Router == null)
                {
                    throw new NotFoundException("未找到资源");
                }

                IBehavior behavior = null;
                while ((behavior = Router.Next(path)) != null)
                {
                   
                    Invoke(behavior);
                }
                //throw new Exception("1");
            }
            catch (Exception ex)
            {
                //Assert.E(ex);
                
                throw ex;
            }

        }

        protected void Invoke(IBehavior behavior)
        {
            var verbAttrs = behavior.GetType().GetTypeInfo().GetCustomAttribute(typeof(VerbAttribute)) as VerbAttribute;

            if (verbAttrs == null || verbAttrs.Verb.HasFlag(Verb))
            {
                //string text = middleware.Text;
                //身份验证
                Authentic(behavior);

                behavior.Invoke().Wait();
                return;
            }
            //Assert.W("该中间件不支持相应谓词操作");
            throw new NotFoundException("该中间件不支持相应谓词操作");
        }


    }
}
