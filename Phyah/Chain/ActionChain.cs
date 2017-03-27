using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phyah.Chain
{
    public class ActionChain<T>
    {
        public static ActionChain<T> Chain(params Action<T>[] action)
        {
            var actionChain = new ActionChain<T>();
            foreach (var act in action)
            {
                actionChain.Action(act);
            }
            return actionChain;
        }
        private Queue<Action<T>> _Queue;
        public ActionChain()
        {
            _Queue = new Queue<Action<T>>();
        }
        public ActionChain<T> Action(Action<T> action)
        {
            _Queue.Enqueue(action);
            return this;
        }
        public void Invoke(T value)
        {
            if (_Queue == null) { return; }
            if (_Queue.Count == 0) { return; }
            Action<T> temp = default(Action<T>);
            while ((temp = _Queue.Dequeue()) != null)
            {
                temp.Invoke(value);
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
