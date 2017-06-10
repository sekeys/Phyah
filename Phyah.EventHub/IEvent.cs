

namespace Phyah.EventHub
{
    using Phyah.Collection;
    public interface IEvent
    {
        object Sender { get; }
        object[] Arguments { get; }
    }
}
