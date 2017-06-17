

namespace Phyah.Web
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Phyah.Interface;
    //using Phyah.Debug;

    //[Assert("Anonymouse Middleware...")]
    public class AnonymousBehavior : IBehavior
    {
        public string Text { get; private set; }

        public bool AllowDebug { get; private set; }
        public Type Type { get; private set; }
        public async Task Invoke()
        {
            await Task.Run(() =>
             {
                 var ti = Type.GetTypeInfo();
                 var authes = ti.GetCustomAttributes(typeof(Phyah.Authentication.AuthenticationAttribute)) as IEnumerable<Phyah.Authentication.AuthenticationAttribute>;
                 foreach (var item in authes)
                 {
                     if (item is Phyah.Authentication.IAuthentication)
                     {
                         ((Phyah.Authentication.IAuthentication)(item)).Authentic();
                     }
                     else
                     {
                         ((Phyah.Authentication.IAuthentication)Activator.CreateInstance(item.Type)).Authentic();
                     }
                 }
                 //if (Process.ProcessContext.Current.Debug && AllowDebug)
                 //{
                 //    var debugAttribute = ti.GetCustomAttribute(typeof(IDebugAttribute)) as IEnumerable<Phyah.Debug.IDebugAttribute>;
                 //    foreach (var debug in debugAttribute)
                 //    {
                 //        debug.Assert();
                 //    }
                 //}

                 object obj = Activator.CreateInstance(Type);

                 ti.GetMethod("Invoke").Invoke(obj, null);
             });
        }

        public AnonymousBehavior(object obj)
        {
            Type = obj.GetType();
            string name = Type.Name;
            name = Web.Pattern.UseStrict ? name : name.ToLower();
            if (name.EndsWith("Behavior", StringComparison.OrdinalIgnoreCase))
            {
                Text = name.Substring(0, name.Length - 10);
            }

        }
    }
}
