

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
    using Phyah.Web.Attributes;
    using Phyah.Authentication;
    using System.Linq;

    public class DefaultProcessor : Processor
    {
        private IRouter Router;
        IPath path => AccessorContext.DefaultContext.Get<IPath>();
        static Type TaskType => typeof(Task);
        private void InvokeMethod(MethodInfo rest,object invoker,object[] args)
        {
            var attributes = rest.GetCustomAttributes().Where(m => m is IAuthentication) as IEnumerable<IAuthentication>;
            if (attributes != null)
            {
                foreach (var attribute in attributes)
                {
                    attribute.Authentic();
                }
            }
            if (rest != null)
            {
                if (rest.ReturnType == TaskType)
                {
                    var task = rest.Invoke(invoker, args) as Task;
                    if (task != null)
                    {
                        task.Wait();
                    }
                }
                else
                {
                    rest.Invoke(invoker, args);
                }
            }
        }
        public override void Process()
        {
            try
            {
                Router = RouterManager.Manager.Routing(null);
                if (Router == null)
                {
                    throw new NotFoundException("未找到资源");
                }
                IBehavior behavior = null;
                TypeInfo type = null;
                while ((behavior = Router.Next(path)) != null)
                {
                    type = behavior.GetType().GetTypeInfo();
                    RouterOnMethodAttribute routerOnMethodAttr = type.GetCustomAttribute<RouterOnMethodAttribute>();
                    if (path.Current.Next != null && routerOnMethodAttr != null)
                    {
                        var rest = type.GetMethod(routerOnMethodAttr.Method, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
                        if (rest == null)
                        {
                            throw new StatusException($"{nameof(behavior)}内部未实现RouteOnMethod接口", 404);
                        }
                        string raw = rest.Invoke(behavior, new object[] { path }) as string;
                        if (string.IsNullOrWhiteSpace(raw))
                        {
                            break;
                            //throw new StatusException($"{nameof(behavior)}内部未制定具体路由", 404);
                        }
                        rest = type.GetMethod(raw, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
                        if (rest == null)
                        {
                            throw new StatusException("未找相关的实现方法", 404);
                        }
                        InvokeMethod(rest,behavior,null);
                        break;
                    }
                    else
                    {
                        Invoke(behavior);
                    }
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
