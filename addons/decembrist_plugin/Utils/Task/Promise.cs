#nullable enable
using System;
using System.Threading.Tasks;

namespace Decembrist.Utils.Task
{
    public class Promise<T>
    {
        private readonly TaskCompletionSource<T?> _promise = new();
        private readonly Action<Action<T?>, Action<Exception>> _block;

        private bool _started;

        public Promise(Action<Action<T?>, Action<Exception>> block)
        {
            _block = block;
        }

        public Task<T> Start()
        {
            if (!_started)
            {
                _block(
                    @object => _promise.TrySetResult(@object),
                    ex => _promise.TrySetException(ex)
                );
                _started = true;
            }
            else
            {
                throw new Exception("Promise started already");
            }

            return _promise.Task;
        }
    }
}