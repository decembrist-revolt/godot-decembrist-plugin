using Godot;

namespace Decembrist.Utils
{
    public static class InputEvents
    {
        public static bool IsLeftMouseDown(InputEvent @event) => @event is InputEventMouseButton
        {
            ButtonIndex: (int) ButtonList.Left, 
            Pressed: true
        };
        
        public static bool IsLeftMouseUp(InputEvent @event) => @event is InputEventMouseButton
        {
            ButtonIndex: (int) ButtonList.Left, 
            Pressed: false
        };

        public static bool IsMouseMove(InputEvent @event) => @event is InputEventMouseMotion;
    }
}