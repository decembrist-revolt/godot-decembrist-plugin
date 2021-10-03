using System;
using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class LineEdits
    {
        public const string TextChangeRejectedSignal = "text_change_rejected";
        public const string TextChangedSignal = "text_changed";
        public const string TextEnteredSignal = "text_entered";
        
        /// <summary>
        /// Emitted when trying to append text that would overflow the max_length.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnTextChangeRejected(this LineEdit edit, VoidFunc action)
        {
            var callback = edit.Subscribe(TextChangeRejectedSignal, action);
            return () => edit.Unsubscribe(TextChangeRejectedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the text changes.
        /// </summary>
        /// <param name="edit">This LineEdit</param>
        /// <param name="action">text_changed ( String new_text )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnTextChanged(this LineEdit edit, Action<string> action)
        {
            var callback = edit.Subscribe(TextChangedSignal, action);
            return () => edit.Unsubscribe(TextChangedSignal, callback);
        }
        
        /// <summary>
        /// Emitted when the user presses @GlobalScope.KEY_ENTER on the LineEdit.
        /// </summary>
        /// <param name="edit">This LineEdit</param>
        /// <param name="action">text_changed ( String new_text )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnTextEntered(this LineEdit edit, Action<string> action)
        {
            var callback = edit.Subscribe(TextEnteredSignal, action);
            return () => edit.Unsubscribe(TextEnteredSignal, callback);
        }
    }
}