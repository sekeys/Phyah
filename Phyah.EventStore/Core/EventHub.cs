using Phyah.Concurrency;
using Phyah.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.EventStore
{
    public abstract class EventHub : IHub
    {
        /// <summary>
        /// 执行器
        /// </summary>
        readonly Executor Schedule;

        /// <summary>
        /// 工作
        /// </summary>
        public readonly Executor Worker;

        public EventHub(Executor schedule, Executor worker)
        {
            this.Schedule = schedule;
            this.Worker = worker;
        }


        public void Broadcast(string name, EventArgs eventArgs)
        {
            var context = GlobalVariable.Context;
            Schedule.Schedule(new EventRunnable(eventArgs, context, this), TimeSpan.Zero);
        }

        public Task BroadcastAsync(string name, EventArgs eventArgs)
        {
            var context = GlobalVariable.Context;
            return Schedule.ScheduleAsync(() =>
            {
                try
                {
                    var executor = GlobalVariable.EventStore.Match(name);
                    this.Worker.ScheduleAsync(() =>
                    {
                        executor.Execute(context);
                    }, TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }, TimeSpan.Zero);
        }

        public void Subscribe(string name, IEventHandler handler)
        {
            Schedule.Schedule(new EventSubscribe(name, handler), TimeSpan.Zero);
        }
        public void Unsubscribe(string name, IEventHandler handler)
        {
            Schedule.Schedule(new EventUnsubscribe(name, handler), TimeSpan.Zero);
        }
        public void Unsubscribe(EventDescriptor description)
        {
            Schedule.Schedule(() => { GlobalVariable.EventStore.Unstore(description.EventName, description); }, TimeSpan.Zero);
        }
        public void Subscribe(EventDescriptor description)
        {
            Schedule.Schedule(() => { GlobalVariable.EventStore.Store(description.EventName, description); }, TimeSpan.Zero);
        }

        public Task SubscribeAsync(EventDescriptor description)
        {
            return Schedule.ScheduleAsync(() => { GlobalVariable.EventStore.Store(description.EventName, description); }, TimeSpan.Zero);
        }
        public Task SubscribeAsync(string name, IEventHandler handler)
        {
            return Schedule.ScheduleAsync(() =>
            {
                var descriptor = EventDescriptor.Descriptions(name, handler);
                GlobalVariable.EventStore.Store(name, descriptor);
            }, TimeSpan.Zero);
        }
        public Task UnsubscribeAsync(EventDescriptor description)
        {
            return Schedule.ScheduleAsync(() => { GlobalVariable.EventStore.Unstore(description.EventName, description); }, TimeSpan.Zero);
        }
        public Task UnsubscribeAsync(string name, IEventHandler handler)
        {
            return Schedule.ScheduleAsync(() =>
            {
                var descriptor = EventDescriptor.Descriptions(name, handler);
                GlobalVariable.EventStore.Unstore(name, descriptor);
            }, TimeSpan.Zero);
        }


        public void Register(string name)
        {
            Schedule.Schedule(new EventRegister(name), TimeSpan.Zero);
        }
        public void Unregister(string name)
        {
            Schedule.Schedule(new EventUnregister(name), TimeSpan.Zero);
        }

        public Task RegisterAsync(string name)
        {
            return Schedule.ScheduleAsync(() =>
            {
                GlobalVariable.EventStore.Register(name);
            }, TimeSpan.Zero);
        }
        public Task UnregisterAsync(string name)
        {
            return Schedule.ScheduleAsync(() => { GlobalVariable.EventStore.Unregister(name); }, TimeSpan.Zero);
        }

        internal class EventRunnable : IRunnable
        {
            public EventArgs Event { get; private set; }
            public EventContext Context { get; private set; }
            public EventStore Stores => GlobalVariable.EventStore;
            public EventHub Hub { get; set; }
            public EventRunnable(EventArgs evnt, EventContext context, EventHub hub)
            {
                Event = evnt;
                Context = context;
                Hub = hub;
            }
            public void Run()
            {
                var executor = Stores.Match(this.Context.EventName);
                this.Hub.Worker.ScheduleAsync(() =>
                {
                    executor.Execute(Context);
                }, TimeSpan.Zero);
            }
        }
        internal class EventSubscribe : IRunnable
        {
            IEventHandler Handler;
            string Name;
            public EventSubscribe(string name, IEventHandler handler)
            {
                Handler = handler;
                Name = name;

            }
            public void Run()
            {
                var descriptor = EventDescriptor.Descriptions(Name, Handler);
                GlobalVariable.EventStore.Store(Name, descriptor);
            }

        }
        internal class EventUnsubscribe : IRunnable
        {
            IEventHandler Handler;
            string Name;
            public EventUnsubscribe(string name, IEventHandler handler)
            {
                Handler = handler;
                Name = name;

            }
            public void Run()
            {
                var descriptor = EventDescriptor.Descriptions(Name, Handler);
                GlobalVariable.EventStore.Unstore(Name, descriptor);
            }

        }

        internal class EventRegister : IRunnable
        {
            string Name;
            public EventRegister(string name)
            {
                Name = name;

            }
            public void Run()
            {
                GlobalVariable.EventStore.Register(Name);
            }
        }

        internal class EventUnregister : IRunnable
        {
            string Name;
            public EventUnregister(string name)
            {
                Name = name;

            }
            public void Run()
            {
                GlobalVariable.EventStore.Unregister(Name);
            }

        }

    }
}