using System;
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

    public static class OptionButtons
    {
        public const string ItemFocusedSignal = "item_focused";
        public const string ItemSelectedSignal = "item_selected";
        
        /// <summary>
        /// Emitted when the user navigates to an item using the ui_up or ui_down actions.
        /// The index of the item selected is passed as argument.
        /// </summary>
        /// <param name="button">This button</param>
        /// <param name="action">action( int index )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnItemFocused(this OptionButton button, Action<int> action)
        {
            var callback = button.Subscribe(ItemFocusedSignal, action);
            return () => button.Unsubscribe(ItemFocusedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the current item has been changed by the user.
        /// The index of the item selected is passed as argument.
        /// </summary>
        /// <param name="button">This button</param>
        /// <param name="action">action( int index )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnItemSelected(this OptionButton button, Action<int> action)
        {
            var callback = button.Subscribe(ItemSelectedSignal, action);
            return () => button.Unsubscribe(ItemSelectedSignal, callback);
        }
    }
}