using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phyah.EventPipeline
{
    using Phyah.TypeContainer;
    using Phyah.Extensions;

    public class Variable
    {
        private static bool HasInited = false;
        static object locker = new object();
        static Variable()
        {
            if (!HasInited)
            {
                lock (locker)
                {
                    if (HasInited)
                    {
                        return;
                    }
                    TypeContainer.Container.Inject(typeof(Acceptor), typeof(Acceptor));
                    acceptor = new Acceptor();
                     HasInited = true;
                }
            }
        }
        private static Acceptor acceptor;
        public static Acceptor Acceptor => acceptor;
        
    }
}