

namespace Phyah.Web
{
    using System;
    using Phyah.Collection;
    using Phyah.Interface;

    public abstract class PathMapping : IMapping<IPath, IPath>
    {
        public int Priority { get; set; } = 0;
        public abstract bool IsMatch(IPath path);
        public abstract IPath Map(IPath value);
    }
}
