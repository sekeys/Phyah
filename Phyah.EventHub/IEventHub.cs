
namespace Phyah.EventHub
{
    using System;
    using System.Threading.Tasks;

    public interface IEventHub
    {
        void Subject(IReceptor receptor);
        void Subject<T>() where T : IReceptor;
        void Subject(string eventName, IReceptor receptor);
        void Broadcast(string eventName, IEvent evnt);
        void Broadcast<T>(string eventName, IEvent evnt);
        void Broadcast<T>(IEvent evnt) where T : IReceptor;
        void Broadcast(Type type, IEvent evnt);
        Task BroadcastAsync(string eventName, IEvent evnt);
        Task BroadcastAsync(Type type, IEvent evnt);
        Task BroadcastAsync<T>(IEvent evnt);

    }
}
