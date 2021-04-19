using System;
using Godot;
using Decembrist.Utils.Callback;

namespace Decembrist.Utils
{
    /**
     * Tween class wrapper
     */
    public static class Tweens
    {
        public static void InterpolateProperty(
            Tween tween,
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