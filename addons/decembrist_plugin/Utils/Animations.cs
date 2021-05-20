using Decembrist.Utils.Task;
using Godot;

namespace Decembrist.Utils
{
    public static class Animations
    {
        public static AnimationPromise Show(this AnimationPlayer animObject, string animName) =>
            new AnimationPromise(animObject).Show(animName);
    }
}