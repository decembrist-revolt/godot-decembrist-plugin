using System;
using Decembrist.Utils.Callback;
using Godot;

namespace Decembrist.Utils
{
    public static class Controls
    {
        public static AbstractCallback OnPointerEnter(this Control control, Action callback)
        {
            return control.Subscribe("mouse_entered", callback);
        }
        
        public static AbstractCallback OnPointerExit(this Control control, Action callback)
        {
            return control.Subscribe("mouse_exited", callback);
        }
        
        public static AbstractCallback OnGuiInput(this Control control, Action<InputEvent> callback)
        {
            return control.Subscribe("gui_input", callback);
        }

        public static AbstractCallback OnGuiInput(
            this Control control,
            Func<InputEvent, bool> eventPredicate,
            Action<InputEvent> callback)
        {
            return control.Subscribe("gui_input", (InputEvent @event) =>
            {
                if (eventPredicate(@event))
                {
                    callback(@event);
                }
            });
        }
    }
}