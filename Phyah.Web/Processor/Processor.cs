

namespace Phyah.Web
{
    using System;
    using System.Threading.Tasks;
    using System.Reflection;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Phyah.Concurrency;
    using Phyah.Enumerable;
    using Phyah.Interface;

    public abstract class Processor
    {
        readonly IPath path;
        public IPath Path => path;
        readonly HttpContext httpContext;
        public HttpContext HttpContext => httpContext;
        public static Processor Current => AccessorContext.DefaultContext.Get<Processor>("PROCESSOR.DEFAULT");
        public Processor()
        {
            httpContext = AccessorContext.DefaultContext.Get<HttpContext>();
            path = AccessorContext.DefaultContext.Get<IPath>();
            //Verb =(Verbs) Enum.Parse(typeof(Verbs), HttpContext.Request.Method,true);

            InitializeVerb();
            AccessorContext.DefaultContext.Set("PROCESSOR.DEFAULT", this);
        }
        private void InitializeVerb()
        {
            switch (HttpContext.Request.Method.ToLower())
            {
                case "get": Verb = Verbs.Get; break;
                case "post": Verb = Verbs.Post; break;
                case "head": Verb = Verbs.Head; break;
                case "delete": Verb = Verbs.Delete; break;
                case "options": Verb = Verbs.Options; break;
                case "put": Verb = Verbs.Put; break;
                case "unsafe": Verb = Verbs.Unsafe; break;
                case "auth": Verb = Verbs.Auth; break;
                //case "get": Verb = Verbs.Get; break;
                //case "get": Verb = Verbs.Get; break;
                default: Verb = Verbs.Other; break;
            }
        }
        public abstract void Process();

        public Verbs Verb { get; private set; }
        //protected abstract void Invoke();

        protected virtual void Authentic(IBehavior middle)
        {
            var ti = middle.GetType().GetTypeInfo();
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
        }
    }
}