

namespace Phyah.Web
{
    using System.Threading.Tasks;
    public interface IBehavior
    {
        string Text { get; }
        Task Invoke();
    }
}