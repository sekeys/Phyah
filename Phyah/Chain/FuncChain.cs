using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Chain
{
    public class FuncChain<T>
    {
        public static FuncChain<T> Chain(params Func<T, T>[] funcs)
        {
            var actionChain = new FuncChain<T>();
            foreach (var act in funcs)
            {
                actionChain.Action(act);
            }
            return actionChain;
        }
        private readonly Queue<Func<T, T>> _Queue;
        public FuncChain()
        {
            _Queue = new Queue<Func<T, T>>();
        }
        public FuncChain<T> Action(Func<T, T> action)
        {
            _Queue.Enqueue(action);
            return this;
        }
        public T Invoke(T value)
        {
            if (_Queue == null) { return default(T); }
            Func<T, T> temp = default(Func<T, T>);
            while (_Queue.Count > 0 && (temp = _Queue.Dequeue()) != null)
            {
                value = temp.Invoke(value);
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
