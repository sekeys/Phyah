

namespace Phyah.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Phyah.Interface;
    public class AppSetting : Parameter, IParameter
    {
        private static AppSetting _Setting;
        private static object Locker = new object();
        public static AppSetting AppSettings
        {
            get
            {
                if (_Setting == null)
                {
                    lock (Locker)
                    {
                        if (_Setting == null) _Setting = new AppSetting();
                    }
                }
                return _Setting;
            }
        }
    }
}
