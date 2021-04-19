using Godot;
using Decembrist.Utils.Task;

namespace Decembrist.Utils
{
    public static class Animations
    {
        public static AnimationPromise Show(AnimationPlayer animObject, string animName) =>
            new AnimationPromise(animObject).Show(animName);
    }
}