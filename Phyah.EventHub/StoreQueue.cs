using System;
using Phyah.Collection;

namespace Phyah.EventHub
{
    public class StoreQueue<T> : IQueue<IReceptor>
    {
        public int Count => throw new NotImplementedException();

        public bool IsEmpty => throw new NotImplementedException();

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IQueue<IReceptor> Clone()
        {
            throw new NotImplementedException();
        }

        public IReceptor Dequeue()
        {
            throw new NotImplementedException();
        }

        public void Enqueue(IReceptor item)
        {
            throw new NotImplementedException();
        }

        public IReceptor Peek()
        {
            throw new NotImplementedException();
        }
    }
}