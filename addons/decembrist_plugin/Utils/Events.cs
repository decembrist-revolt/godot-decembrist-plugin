using Godot;

namespace Decembrist.Utils
{
    public static class Events
    {
        public static bool IsLeftMouseDownEvent(this InputEvent @event)
        {
            return @event is InputEventMouseButton {ButtonIndex: (int) ButtonList.Left, Pressed: false};
        }
    }
}