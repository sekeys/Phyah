using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGraph
{
    public class EventUpgraderModel : EventModel
    {
        /// <summary>
        /// Subscribe类型
        /// </summary>
        public string OldSubscribe
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// Subscribe类型
        /// </summary>
        public string NewSubscribe
        {
            get => default(int);
            set
            {
            }
        }

        /// <summary>
        /// Subscribe类型Url
        /// </summary>
        public int NewUrl
        {
            get => default(int);
            set
            {
            }
        }
    }
}