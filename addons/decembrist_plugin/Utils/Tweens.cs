using System;
using Decembrist.Utils.Callback;
using Godot;

namespace Decembrist.Utils
{
    /**
     * Tween class wrapper
     */
    public static class Tweens
    {
        public static void InterpolateProperty(
            this Tween tween,
            Godot.Object obj,
            NodePath property,
            object initialVal,
            object finalVal,
            float duration,
            Tween.TransitionType transType = Tween.TransitionType.Linear,
            Tween.EaseType easeType = Tween.EaseType.InOut,
            float delay = 0.0f)
        {
            tween.InterpolateProperty(obj, property, initialVal, finalVal, duration, transType, easeType, delay);
        }

        public static void HandleOnComplete(Tween tween, Action<Godot.Object, NodePath> callback)
        {
            tween.Subscribe("tween_completed", callback);
        }
        
        public static void HandleOnStart(Tween tween, Action<Godot.Object, NodePath> callback)
        {
            tween.Subscribe("tween_started", callback);
        }

    }
}