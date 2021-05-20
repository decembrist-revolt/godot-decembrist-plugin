using System;
using Decembrist.Utils.Callback;
using Godot;

namespace Decembrist.Utils
{
    public static class Areas
    {

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