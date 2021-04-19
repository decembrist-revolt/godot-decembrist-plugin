using System;
using Godot;
using Decembrist.Utils.Callback;

namespace Decembrist.Utils
{
    public static class Areas
    {
        public static AbstractCallback OnPointerEnter(this Area2D area, Action callback)
        {
            return area.Subscribe("mouse_entered", callback);
        }
        
        public static AbstractCallback OnPointerExit(this Area2D area, Action callback)
        {
            return area.Subscribe("mouse_exited", callback);
        }
        
        /// <summary>
        /// Invokes on input event
        /// </summary>
        /// <param name="area">Area</param>
        /// <param name="callback">Callback(Node viewport, InputEvent @event, int shapeIdx)</param>
        /// <returns>Callback object</returns>
        public static AbstractCallback OnInput(this Area2D area, Action<Node, InputEvent, int> callback)
        {
            return area.Subscribe("input_event", callback);
        }
        
    }
}