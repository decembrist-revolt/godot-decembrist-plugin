using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class Buttons
    {
        public const string PressedSignal = "pressed";
        public const string ButtonDownSignal = "button_down";
        public const string ButtonUpSignal = "button_up";
        
        /// <summary>
        /// On button press event subscription
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnButtonPress(this Button button, VoidFunc action)
        {
            var callback = button.Subscribe(PressedSignal, action);
            return () => button.Unsubscribe(PressedSignal, callback);
        }
        
        /// <summary>
        /// On button down event subscription
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnButtonDown(this Button button, VoidFunc action)
        {
            var callback = button.Subscribe(ButtonDownSignal, action);
            return () => button.Unsubscribe(ButtonDownSignal, callback);
        }
        
        /// <summary>
        /// On button up event subscription
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnButtonUp(this Button button, VoidFunc action)
        {
            var callback = button.Subscribe(ButtonUpSignal, action);
            return () => button.Unsubscribe(ButtonUpSignal, callback);
        }
    }
}