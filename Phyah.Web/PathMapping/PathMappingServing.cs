using Phyah.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Web
{
    public class PathMappingServing
    {
        public List<PathMapping> Mappings { get; private set; } = new List<PathMapping>();

        public void Register(PathMapping pathMapping)
        {
            Mappings.Add(pathMapping);
        }
        public void Unregister(PathMapping pathMapping)
        {
            Mappings.Remove(pathMapping);
        }
        public void Clear()
        {
            Mappings.Clear();
        }

        public IPath Mapping(IPath path)
        {
            if (!Enable)
            {
                return path;
            }
            foreach (var item in Mappings)
            {
                if (item.IsMatch(path))
                {
                    return item.Map(path);
                }
            }
            if (DefaultPathMapping.Instance.IsMatch(path))
            {
                return DefaultPathMapping.Instance.Map(path);
            }
            return path;
        }

        private static PathMappingServing _Instance;
        private static object locker = new object();

        public bool Enable { get; set; }
        public static PathMappingServing Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (locker)
                    {
                        if (_Instance == null)
                            _Instance = new PathMappingServing();
                    }
                }
                return _Instance;
            }
        }
    }
}
