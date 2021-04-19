using System;

namespace Decembrist.Utils.Callback
{
    public class GodotAction2Callback<T1, T2> : AbstractCallback
    {
        private readonly Action<T1, T2> _callback;

        public GodotAction2Callback(Action<T1, T2> callback)
        {
            _callback = callback;
        }

        public void Invoke(T1 first, T2 second)
        {
            _callback(first, second);
        }
    }
}