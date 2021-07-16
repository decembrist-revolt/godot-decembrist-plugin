using System;
using Decembrist.Utils.Callback;
using Godot;
using VoidFunc = System.Action;

namespace Decembrist.Utils
{
    public static class Controls
    {
        public const string FocusEnteredSignal = "focus_entered";
        public const string FocusExitedSignal = "focus_exited";
        public const string GuiInputSignal = "gui_input";
        public const string MinimumSizeChangedSignal = "minimum_size_changed";
        public const string ModalClosedSignal = "modal_closed";
        public const string MouseEnteredSignal = "mouse_entered";
        public const string MouseExitedSignal = "mouse_exited";
        public const string ResizedSignal = "resized";
        public const string SizeFlagsChangedSignal = "size_flags_changed";

        /// <summary>
        /// Emitted when the node gains keyboard focus.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnFocusEntered(this Control control, VoidFunc action)
        {
            var callback = control.Subscribe(FocusEnteredSignal, action);
            return () => control.Unsubscribe(FocusEnteredSignal, callback);
        }

        /// <summary>
        /// Emitted when the node loses keyboard focus.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnFocusExited(this Control control, VoidFunc action)
        {
            var callback = control.Subscribe(FocusExitedSignal, action);
            return () => control.Unsubscribe(FocusExitedSignal, callback);
        }

        /// <summary>
        /// Emitted when the node receives an InputEvent.
        /// </summary>
        /// <param name="control">This control</param>
        /// <param name="action">action ( InputEvent event )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnGuiInput(this Control control, Action<InputEvent> action)
        {
            var callback = control.Subscribe(GuiInputSignal, action);
            return () => control.Unsubscribe(GuiInputSignal, callback);
        }

        /// <summary>
        /// Emitted when the node receives an InputEvent.
        /// </summary>
        /// <param name="control">This control</param>
        /// <param name="inputPredicate">Invokes only if the event satisfies the predicate</param>
        /// <param name="action">action ( InputEvent event )</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnGuiInput(
            this Control control,
            Func<InputEvent, bool> inputPredicate,
            Action<InputEvent> action)
        {
            var callback = control.Subscribe(GuiInputSignal, (InputEvent @event) =>
            {
                if (inputPredicate(@event))
                {
                    action(@event);
                }
            });
            return () => control.Unsubscribe(GuiInputSignal, callback);
        }

        /// <summary>
        /// Emitted when the node receives an mouse InputEvent.
        /// </summary>
        /// <param name="control">This control</param>
        /// <param name="action">action ( InputEventMouseButton event )</param>
        /// <param name="inputPredicate">Invokes only if the event satisfies the predicate</param>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnMouseInput(
            this Control control,
            Action<InputEventMouseButton> action,
            Func<InputEventMouseButton, bool> inputPredicate = null) => control.OnGuiInput(
            @event => @event is InputEventMouseButton mouseEvent
                      && (inputPredicate == null || inputPredicate(mouseEvent)),
            @event => action(@event as InputEventMouseButton)
        );


        /// <summary>
        /// Emitted when the node's minimum size changes.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnMinimumSizeChanged(this Control control, Action action)
        {
            var callback = control.Subscribe(MinimumSizeChangedSignal, action);
            return () => control.Unsubscribe(MinimumSizeChangedSignal, callback);
        }

        /// <summary>
        /// Emitted when a modal Control is closed. See show_modal.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnModalClosed(this Control control, Action action)
        {
            var callback = control.Subscribe(ModalClosedSignal, action);
            return () => control.Unsubscribe(ModalClosedSignal, callback);
        }

        /// <summary>
        /// Emitted when the mouse enters the control's Rect area, provided its mouse_filter lets the event reach it.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnMouseEntered(this Control control, Action action)
        {
            var callback = control.Subscribe(MouseEnteredSignal, action);
            return () => control.Unsubscribe(MouseEnteredSignal, callback);
        }

        /// <summary>
        /// Emitted when the mouse leaves the control's Rect area, provided its mouse_filter lets the event reach it.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnMouseExited(this Control control, Action action)
        {
            var callback = control.Subscribe(MouseExitedSignal, action);
            return () => control.Unsubscribe(MouseExitedSignal, callback);
        }

        /// <summary>
        /// Emitted when the control changes size.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnResized(this Control control, Action action)
        {
            var callback = control.Subscribe(ResizedSignal, action);
            return () => control.Unsubscribe(ResizedSignal, callback);
        }

        /// <summary>
        /// Emitted when one of the size flags changes. See size_flags_horizontal and size_flags_vertical.
        /// </summary>
        /// <returns>Unsubscribe callback</returns>
        public static VoidFunc OnSizeFlagsChanged(this Control control, Action action)
        {
            var callback = control.Subscribe(SizeFlagsChangedSignal, action);
            return () => control.Unsubscribe(SizeFlagsChangedSignal, callback);
        }
    }
}