using System;
using Godot;
using Decembrist.Utils.Callback;

namespace Decembrist.Utils
{
    public static class Controls
    {
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