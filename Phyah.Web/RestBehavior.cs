

namespace Phyah.Web
{
    using Phyah.Authentication;
    using Phyah.Web.Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    public abstract class RestBehavior : Behavior
    {
        static Type TaskType => typeof(Task);
        public override Task Invoke()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var rest = this.GetType().GetMethod(Verb.ToString(), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
                    if (rest == null)
                    {
                        throw new StatusException("未找相关的实现方法", 404);
                    }
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
                            var task = rest.Invoke(this, null) as Task;
                            if (task != null)
                            {
                                task.Wait();
                            }
                        }
                        else
                        {
                            rest.Invoke(this, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    await this.Status(505, ex.ToString());
                }
            });
        }
    }
}
