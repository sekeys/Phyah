using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventPipeline.Messages
{
    using System.Net;
    public class HttpMessage:Message
    {
        public CookieCollection Cookie { get;private set; }
        public HttpMessage()
        {
            Cookie = new CookieCollection();
        }
    }
}
