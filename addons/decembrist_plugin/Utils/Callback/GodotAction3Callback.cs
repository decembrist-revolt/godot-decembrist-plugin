using System;
using Godot;

namespace Decembrist.Utils.Callback
{
    public class GodotAction3Callback<T1, T2, T3> : AbstractCallback
    {
        private readonly Action<T1, T2, T3> _callback;

        public GodotAction3Callback(Action<T1, T2, T3> callback)
        {
            _callback = callback;
        }

        public void Invoke(T1 first, T2 second, T3 third)
        {
            _callback(first, second, third);
        }
    }
}