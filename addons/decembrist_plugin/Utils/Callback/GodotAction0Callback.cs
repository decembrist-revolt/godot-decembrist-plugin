using System;

namespace Decembrist.Utils.Callback
{
    public class GodotActionCallback : AbstractCallback
    {
        private readonly Action _callback;

        public GodotActionCallback(Action callback)
        {
            _callback = callback;
        }

        public void Invoke()
        {
            _callback();
        }
    }
}