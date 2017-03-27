using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Chain
{
    public class AsyncFuncChain<T>
    {
        public static AsyncFuncChain<T> Chain(params Func<T, T>[] funcs)
        {
            var actionChain = new AsyncFuncChain<T>();
            foreach (var act in funcs)
            {
                actionChain.Action(act);
            }
            return actionChain;
        }
        private readonly Queue<Func<T, T>> _Queue;
        public AsyncFuncChain()
        {
            _Queue = new Queue<Func<T, T>>();
        }
        public AsyncFuncChain<T> Action(Func<T, T> action)
        {
            lock (_Queue)
            {
                _Queue.Enqueue(action);
            }
            return this;
        }
        public T Invoke(T value)
        {
            if (_Queue == null) { return value; }
            if (_Queue.Count == 0) { return value; }

            Func<T, T> temp = default(Func<T, T>);
            while (true)
            {
                lock (_Queue)
                {
                    temp = _Queue.Dequeue();
                    if (_Queue.Count == 0) { break; }
                }
                if (temp != null)
                {
                    value = temp(value);
                }
            }
            return value;
        }

        public async Task<T> InvokeAsync(T value)
        {
            return await Task.Run<T>(() =>
            {
                return this.Invoke(value);
            });
        }
    }
}
