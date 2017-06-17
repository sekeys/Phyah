

namespace Phyah.Web.Handler
{
    using Phyah.Concurrency;
    public class DebugHandler : WebHandler
    {
        protected override void HandleCore()
        {
            Initialize();
            
        }
        public void Initialize()
        {
            var cookie = HttpContext.Request.Cookies;
            var header = HttpContext.Request.Headers;
            if (header.ContainsKey("debug"))
            {
                AccessorContext.DefaultContext.Set<bool>("debug", true);
                AccessorContext.DefaultContext.Set<string>("debug_au", header["debug_au"]);
            }
        }
    }
}
