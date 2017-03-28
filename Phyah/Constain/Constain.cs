using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah
{
    public class ResponseStatus
    {
        public const int SUCCESS = 200;
        public const int FAILD = 500;
        public const int NOTFOUND = 404;
        public const int FORHIDDEN = 403;
        public const int UNAUTHENTENICATION = 401;
        public const int UNKNOWERROR = 502;
        public const int IGNOREERROR = 405;
        public const int APPERROR = 501;
        public const int REDIRECT = 300;
        public const int NOTALLOW = 405;
        public const int ANERROR = 506;
        public const int DEBUG = 199;
        public const int DEBUGERROR = 502;
        public const int BADREQUEST=400;
    }
    
}
