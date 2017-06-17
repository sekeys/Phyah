using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.EventStore
{
    public interface IHub
    {
        /// <summary>
        /// 发布信息
        /// </summary>
        /// <param name="name">事件名称</param>
        /// <param name="eventArgs">事件参数</param>
        void Broadcast(string name, EventArgs eventArgs);
        /// <summary>
        /// 广播事件
        /// </summary>
        Task BroadcastAsync(string name, EventArgs eventArgs);
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="name">事件名称</param>
        /// <param name="handler">事件处理</param>
        void Subscribe(string name, IEventHandler handler);
        /// <summary>
        /// 事件订阅
        /// </summary>
        /// <param name="description">事件描述</param>
        void Subscribe(EventDescriptor description);

        /// <param name="name">事件名称</param>
        /// <param name="handler">事件处理</param>
        Task SubscribeAsync(string name, IEventHandler handler);
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="name">事件名称</param>
        /// <param name="handler">事件处理</param>
        void Unsubscribe(string name, IEventHandler handler);
        
        /// <summary>
        /// 取消事件订阅
        /// </summary>
        /// <param name="description">事件描述</param>
        Task UnsubscribeAsync(EventDescriptor description);

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="name">事件描述</param>
        Task UnsubscribeAsync(string name, IEventHandler handler);
        void Register(string name);
        void Unregister(string name);
        Task RegisterAsync(string name);
        Task UnregisterAsync(string name);
    }
}