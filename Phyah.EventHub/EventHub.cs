



namespace Phyah.EventHub
{
    using Phyah.Concurrency;
    using Phyah.Interface;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    public abstract class EventHub : IEventHub
    {
        protected readonly IExecutor ScheduleExecutor;
        protected readonly IExecutor WorkExecutor;
        protected readonly IReceptorStore Stores;
        public EventHub(IExecutor executor, IReceptorStore store)
        {
            ScheduleExecutor = new IndependentThreadExecutor("Schedule Executor", TimeSpan.Zero);
            WorkExecutor = executor;
            Stores = store;
        }

        public virtual void Broadcast(string eventName, IEvent evnt)
        {
            ScheduleExecutor.Schedule(new FinderOfReceptor(evnt, eventName, Stores, this), TimeSpan.Zero);
        }

        public virtual void Broadcast<T>(string eventName, IEvent evnt) =>
            ScheduleExecutor.Schedule(new FinderOfReceptor<T>(evnt, eventName, Stores, this), TimeSpan.Zero);

        public virtual void Broadcast<T>(IEvent evnt) where T : IReceptor => Broadcast(typeof(T), evnt);

        public virtual void Broadcast(Type type, IEvent evnt)
        {
            ScheduleExecutor.Schedule(new FinderOfReceptorByType(evnt, Stores, this, type), TimeSpan.Zero);
        }

        public virtual Task BroadcastAsync(string eventName, IEvent evnt)
        {
            return
            ScheduleExecutor.ScheduleAsync(
                () =>
                {
                    foreach (var item in Stores.Match(eventName))
                    {
                        item.Accept(evnt);
                    }
                }, TimeSpan.Zero);
        }

        public virtual Task BroadcastAsync(Type type, IEvent evnt)
        {

            return ScheduleExecutor.ScheduleAsync(
                () =>
                {
                    var evntName = type.Name;
                    if (evntName.IndexOf("Receptor") > 0)
                    {
                        evntName = evntName.Substring(0, evntName.Length - 8);
                    }
                    foreach (var item in Stores.Match(evntName))
                    {
                        if (type.IsAssignableFrom(item.GetType()))
                            item.Accept(evnt);
                    }
                }, TimeSpan.Zero);
        }

        public virtual Task BroadcastAsync<T>(IEvent evnt)
        {

            return ScheduleExecutor.ScheduleAsync(
                () =>
                {
                    var evntName = nameof(T);
                    if (evntName.IndexOf("Receptor") > 0)
                    {
                        evntName = evntName.Substring(0, evntName.Length - 8);
                    }
                    foreach (var item in Stores.Match(evntName))
                    {
                        if (item is T)
                            item.Accept(evnt);
                    }
                }, TimeSpan.Zero);
        }

        public virtual void Subject(IReceptor receptor)
        {
            var name = nameof(receptor);
            if (name.IndexOf("Receptor") > 0)
            {
                name = name.Substring(0, name.Length - 8);
            }
            Subject(name, receptor);
        }

        public virtual void Subject<T>() where T : IReceptor
        {
            Subject(Activator.CreateInstance<T>());
        }

        public virtual void Subject(string eventName, IReceptor receptor)
        {
            Stores.Store(eventName, receptor);
        }

        internal class EventTrigger : IRunnable
        {
            public IEvent Event { get; private set; }
            public IReceptor Receptor { get; private set; }

            public EventTrigger(IEvent evnt, IReceptor receptor)
            {
                Event = evnt;
                Receptor = receptor;
            }
            public void Run()
            {
                Receptor.Accept(Event);
            }
        }
        internal class FinderOfReceptor : IRunnable
        {
            public IEvent Event { get; private set; }
            public string EventName { get; private set; }
            public IReceptorStore Stores { get; private set; }
            public EventHub Hub { get; private set; }
            public FinderOfReceptor(IEvent evnt, string evntName, IReceptorStore store, EventHub hub)
            {
                Event = evnt;
                EventName = evntName;
                Stores = store;
                Hub = hub;
            }
            public async void Run()
            {
                foreach (var item in Stores.Match(EventName))
                {
                    await Hub.WorkExecutor.Schedule(new EventTrigger(Event, item), TimeSpan.Zero);
                }
            }
        }
        internal class FinderOfReceptorByType : IRunnable
        {
            public IEvent Event { get; private set; }
            public string EventName { get; private set; }
            public IReceptorStore Stores { get; private set; }
            public EventHub Hub { get; private set; }
            protected readonly Type BaseType;
            public FinderOfReceptorByType(IEvent evnt, IReceptorStore store, EventHub hub, Type baseType)
            {
                Event = evnt;
                Stores = store;
                Hub = hub;
                BaseType = baseType;
                var evntName = BaseType.Name;
                if (evntName.IndexOf("Receptor") > 0)
                {
                    evntName = evntName.Substring(0, evntName.Length - 8);
                }
                EventName = evntName;
            }
            public async void Run()
            {
                foreach (var item in Stores.Match(EventName))
                {
                    if (BaseType.IsAssignableFrom(item.GetType()))
                        await Hub.WorkExecutor.Schedule(new EventTrigger(Event, item), TimeSpan.Zero);
                }
            }
        }
        internal class FinderOfReceptor<T> : IRunnable
        {
            public IEvent Event { get; private set; }
            public string EventName { get; private set; }
            public IReceptorStore Stores { get; private set; }
            public EventHub Hub { get; private set; }
            public FinderOfReceptor(IEvent evnt, string evntName, IReceptorStore store, EventHub hub)
            {
                Event = evnt;
                EventName = evntName;
                Stores = store;
                Hub = hub;
            }
            public async void Run()
            {
                foreach (var item in Stores.Match(EventName))
                {
                    if (item is T)
                        await Hub.WorkExecutor.Schedule(new EventTrigger(Event, item), TimeSpan.Zero);
                }
            }
        }
    }
}