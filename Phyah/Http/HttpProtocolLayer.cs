

namespace Phyah.Http
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Phyah.Enumerable;
    public class HttpProtocolChannel : Channel.ProtocolChannel
    {
        public override Protocols Protocol => Protocols.Http;
       
        protected override void CompletedCore()
        {
            throw new NotImplementedException();
        }

        protected override void OnErrorCore(Exception ex)
        {
            throw new NotImplementedException();
        }

        protected override void OnStartCore()
        {
            throw new NotImplementedException();
        }
    }
}
