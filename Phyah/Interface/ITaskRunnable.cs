

namespace Phyah.Interface
{
    using System.Threading.Tasks;
    public interface ITaskRunnable:IRunnable
    {
        Task Task { get; }
    }
}
