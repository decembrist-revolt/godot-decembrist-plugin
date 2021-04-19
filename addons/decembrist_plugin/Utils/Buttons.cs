using Godot;
using Decembrist.Utils.Callback;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class Buttons
    {
        public static void HandleOnPress(this Button button, VoidFunc onPress)
        {
            var action = new GodotActionCallback(onPress);
            button.Connect("pressed", action, nameof(action.Invoke));
        }
        
        public static void HandleOnDown(this Button button, VoidFunc callback)
        {
            var action = new GodotActionCallback(callback);
            button.Connect("button_down", action, nameof(action.Invoke));
        }
        
        public static void HandleOnUp(this Button button, VoidFunc callback)
        {
            var action = new GodotActionCallback(callback);
            button.Connect("button_up", action, nameof(action.Invoke));
        }
    }
}