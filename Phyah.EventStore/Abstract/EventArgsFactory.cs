


namespace Phyah.EventStore
{
    public abstract class EventArgsFactory
    {
        /// <summary>
        /// 初始化事件参数
        /// </summary>
        public abstract EventArgs Create(Messages.Message message, EventContext context);
    }
}