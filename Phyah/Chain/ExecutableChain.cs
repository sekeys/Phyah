using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Chain
{
    public class ExecutableChain
    {

        private readonly Queue<IExecutable> queue = new Queue<IExecutable>();
        private bool isAcquired = false;
        private bool hasFaulted = false;

        public void Enqueue(IExecutable executable)
        {
            lock (queue)
            {
                if (!hasFaulted)
                {
                    queue.Enqueue(executable);
                }
            }
        }

        public void Dispose()
        {
            lock (queue)
            {
                queue.Clear();
                hasFaulted = true;
            }
        }
        public void Invoke()
        {
            while (true)
            {
                var work = default(IExecutable);
                lock (queue)
                {
                    if (queue.Count > 0)
                        work = queue.Dequeue();
                    else
                    {
                        isAcquired = false;
                        break;
                    }
                }
                try
                {
                    work.Execute();
                }
                catch
                {
                    lock (queue)
                    {
                        queue.Clear();
                        hasFaulted = true;
                    }
                    throw;
                }
            }
        }
        public  Task InvokeAsync()
        {
             return Task.Run(() =>
            {
                Invoke();
            });
        }

    }
}
