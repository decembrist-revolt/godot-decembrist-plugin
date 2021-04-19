using System;
using Godot;

namespace Decembrist.Utils.Callback
{
    public class GodotAction4Callback<T1, T2, T3, T4> : AbstractCallback
    {
        private readonly Action<T1, T2, T3, T4> _callback;

        public GodotAction4Callback(Action<T1, T2, T3, T4> callback)
        {
            _callback = callback;
        }

        public void Invoke(T1 first, T2 second, T3 third, T4 fourth)
        {
            _callback(first, second, third, fourth);
        }
    }
}