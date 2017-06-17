using System;
using System.Collections.Generic;
using System.Text;
using Phyah.Interface;

namespace Phyah.Web
{
    public class DefaultPathMapping : PathMapping
    {
        private static PathMapping _Instance;
        private static object locker = new object();
        public static PathMapping Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (locker)
                    {
                        if (_Instance == null)
                            _Instance = new DefaultPathMapping();
                    }
                }
                return _Instance;
            }
        }
        protected Dictionary<string, string> Mapping = new Dictionary<string, string>();
        public void AddMapping(string key, string value)
        {
            Mapping.Add(key, value);
        }
        public void UpdateMapping(string key, string value)
        {
            Mapping[key] = value;
        }
        public void AddOrUpdate(string key, string value)
        {
            if (Mapping.ContainsKey(key))
                Mapping[key] = value;
            else
                AddMapping(key, value);
        }
        public void RemoveMapping(string key)
        {
            Mapping.Remove(key);
        }
        public override bool IsMatch(IPath path)
        {
            return Mapping.ContainsKey(path.ToString());
        }

        public override IPath Map(IPath path)
        {

            return Path.Parse(Mapping[path.Raw]);
        }
    }
}
