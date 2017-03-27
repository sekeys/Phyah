using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Chain
{


    public class AsyncActionChain<T>
    {
        public static AsyncActionChain<T> Chain(params Action<T>[] action)
        {
            var actionChain = new AsyncActionChain<T>();
            foreach (var act in action)
            {
                actionChain.Action(act);
            }
            return actionChain;
        }
        private Queue<Action<T>> _Queue;
        public AsyncActionChain()
        {
            _Queue = new Queue<Action<T>>();
        }
        public AsyncActionChain<T> Action(Action<T> action)
        {
            lock (_Queue)
            {
                _Queue.Enqueue(action);
            }
            return this;
        }
        public void Invoke(T value)
        {
            if (_Queue == null) { return; }
            if (_Queue.Count == 0) { return; }

            Action<T> temp = default(Action<T>);
            while (true)
            {
                lock (_Queue)
                {
                    if (_Queue.Count == 0) { break; }
                    temp = _Queue.Dequeue();
                }
                if (temp != null)
                {
                    temp(value);
                }
            }

        }
        public void Parallel(T value)
        {
            if (_Queue == null) { return; }
            if (_Queue.Count == 0) { return; }
            _Queue.AsParallel().ForAll(m => m.Invoke(value));
        }
        public async Task InvokeAsync(T value)
        {
            await Task.Run(() =>
            {
                this.Invoke(value);
            });
        }
    }
}
