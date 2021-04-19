using System;
using Godot;

namespace Decembrist.Utils.Callback
{
    public static class Callbacks
    {
        public static AbstractCallback Subscribe(this Godot.Object @object, string signal, Action callback)
        {
            var action = new GodotActionCallback(callback);
            @object.Connect(signal, action, nameof(action.Invoke));
            return action;
        }
        
        public static AbstractCallback Subscribe<T>(this Godot.Object @object, string signal, Action<T> callback)
        {
            var action = new GodotAction1Callback<T>(callback);
            @object.Connect(signal, action, nameof(action.Invoke));
            return action;
        }
        
        public static AbstractCallback Subscribe<T1, T2>(this Godot.Object @object, string signal, Action<T1, T2> callback)
        {
            var action = new GodotAction2Callback<T1, T2>(callback);
            @object.Connect(signal, action, nameof(action.Invoke));
            return action;
        }
        
        public static AbstractCallback Subscribe<T1, T2, T3>(this Godot.Object @object, string signal, Action<T1, T2, T3> callback)
        {
            var action = new GodotAction3Callback<T1, T2, T3>(callback);
            @object.Connect(signal, action, nameof(action.Invoke));
            return action;
        }

        public static void Unsubscribe(this Godot.Object @object, string signal, AbstractCallback callback)
        {
            if (@object.IsConnected(signal, callback, AbstractCallback.InvokeMethod))
            {
                @object.Disconnect(signal, callback, AbstractCallback.InvokeMethod);
            }
        }
    }
}