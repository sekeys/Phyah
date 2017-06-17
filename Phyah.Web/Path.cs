

namespace Phyah.Web
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Phyah.Interface;
    using System.Collections.Concurrent;
    public class Path : IPath
    {
        private string raw;
        public string Raw
        {
            get => raw;
            set { raw = value; Parse(); }
        }

        public IPattern Pattern { get; private set; } 
        public IPattern Current { get; private set; }
        public Path()
        {
            raw = string.Empty;
            Pattern = Web.Pattern.Empty;
            Current = Pattern;
        }
        public static IPath Parse(string v)
        {
            IPath p = new Path();
            if (string.IsNullOrWhiteSpace(v))
            {
                return p;
            }
            else
            {
                p.Raw = v;
            }
            return p;
        }

        public void Parse()
        {
            var patterns=raw.Split('/','\\');
            if (patterns.Length > 0)
            {
                var privouse = Pattern as Pattern;
                foreach(var item in patterns)
                {
                    Pattern pattern = new Pattern(item);
                    privouse.Next = pattern;
                    pattern.Previous = privouse;
                    privouse = pattern;
                }
            }
        }
        
        public bool Next()
        {
            if (Current == null) { return false; }
            Current = Current.Next;
            return Current != null;
        }

        public IPattern Previous()
        {
            return Current.Previous;
        }
    }
}
