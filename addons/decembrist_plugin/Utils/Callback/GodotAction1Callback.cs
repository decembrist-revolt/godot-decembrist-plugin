using System;

namespace Decembrist.Utils.Callback
{
    public class GodotAction1Callback<T> : AbstractCallback
    {
        private readonly Action<T> _callback;

        public GodotAction1Callback(Action<T> callback)
        {
            _callback = callback;
        }

        public void Invoke(T first)
        {
            _callback(first);
        }
    }
}