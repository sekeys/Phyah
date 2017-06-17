using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;

namespace Phyah.Web
{
    public class Pattern:IPattern
    {
        public static IPattern Empty
        {
            get
            {
                Pattern pattern = new Pattern("");
                pattern.Previous = null;
                pattern.Next = null;
                return pattern;
            }
        }
        public static bool UseStrict { get; set; } = false;
        public string Raw { get; private set; }
        public IPattern Next { get; internal protected set; }
        public IPattern Previous { get; internal protected set; }

        public Pattern(string raw)
        {
            Raw = raw;
            if (!UseStrict)
            {
                Raw = Raw.ToLower();
            }
        }
    }
}
