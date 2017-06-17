namespace Phyah.Web
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    public abstract class BehaviorFactory
    {

        public abstract IBehavior Cache(string text);
        public abstract void Cache(IBehavior middle);

        public abstract void Cache<T>();
        public abstract void Cache(Type type);

        public static BehaviorFactory Factory
        {
            get;
            private set;
        }
        public static void Register(BehaviorFactory factory)
        {
            Factory = factory;
        }

        public abstract IEnumerable<KeyValuePair<string, IBehavior>> Caches();
    }

    public class DefaultBehaviorFactory : BehaviorFactory
    {
        protected ConcurrentDictionary<string, IBehavior> Middles = new ConcurrentDictionary<string, IBehavior>();
        public override IEnumerable<KeyValuePair<string, IBehavior>> Caches()
        {
            return Middles.ToArray();
        }
        public override IBehavior Cache(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return null; }
            text = Pattern.UseStrict ? text : text.ToLower();
            var ctx = Concurrency.AccessorContext.DefaultContext.Get<HttpContext>();
            if (Middles.ContainsKey(text))
            {
                return Middles[text];
            }
            text = text + "_" + ctx.Request.Method.ToUpper();
            if (ctx != null && Middles.ContainsKey(text))
            {
                return Middles[text];
            }
            return null;
        }

        public override void Cache(IBehavior middle)
        {
            Middles.TryAdd(middle.Text.ToLower(), middle);
        }

        public override void Cache<T>()
        {
            var type = typeof(T);
            if (type.GetTypeInfo().IsAssignableFrom(typeof(IBehavior)))
            {
                Cache(Activator.CreateInstance(type) as IBehavior);
            }
            else
            {
                Cache(new AnonymousBehavior(Activator.CreateInstance(type)));
            }
        }
        public override void Cache(Type type)
        {
            if (typeof(IBehavior).GetTypeInfo().IsAssignableFrom(type))
            {
                Cache(Activator.CreateInstance(type) as IBehavior);
            }
            else
            {
                Cache(new AnonymousBehavior(Activator.CreateInstance(type)));
            }
        }

    }
}