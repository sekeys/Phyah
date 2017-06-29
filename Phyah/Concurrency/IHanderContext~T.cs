using System;
using System.Collections.Generic;
using System.Text;

namespace Phyah.Concurrency
{
    public interface IHandlerContext<T>
    {
        IExecutor Executor { get; }
        string Name { get; }
        IHandler<T> Handler { get; }
        IEnumerable<Attribute> Attributes { get; }

        IPipeline Pipeline { get; }
        IHandlerContext<T> Next();
        void Handle();
        void Handle(IExecutor executor);
        void Completed();

        void ExceptionCaught(Exception ex);

        void Cancel();
        
    }
}
