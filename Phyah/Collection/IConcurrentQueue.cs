using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Collection
{
    public interface IConcurrentQueue<T>
    {
        bool TryEnqueue(T item);

        bool TryDequeue(out T item);

        bool TryPeek(out T item);

        int Count { get; }

        bool IsEmpty { get; }

        void Clear();

    }
}
