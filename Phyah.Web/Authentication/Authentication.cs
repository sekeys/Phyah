
namespace Phyah.Authentication
{
    using System;
    using Phyah.Interface;
    using System.Collections.Concurrent;
    public class Authentication
    {
        private static Authentication _Instance;
        private static object Locker = new object();
        public static Authentication Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (Locker)
                    {
                        if (_Instance == null) { _Instance = new Authentication(); }
                    }
                }
                return _Instance;
            }
        }


        public ConcurrentBag<IAuthentication> Authentications { get; private set; }
        public void AuthenticOnBeginRequest()
        {
            foreach (var item in Authentications)
            {
                item.Authentic();
            }
        }
        public void AuthenticOnProcessing()
        {
            throw new NotImplementedException();
        }
        protected Authentication()
        {
            Authentications = new ConcurrentBag<IAuthentication>();
        }
    }
}