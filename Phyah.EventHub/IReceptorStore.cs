using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.EventHub
{
    public interface IReceptorStore
    {
        void Store(string name,IReceptor receptor);
        void Store(IReceptor receptor);
        IEnumerable<IReceptor> Match(string name);
        IEnumerable<IReceptor> Match<T>() where T : IReceptor;

        void Unstore(string name);
        void Unstore(string name, IReceptor receptor);
    }
}
