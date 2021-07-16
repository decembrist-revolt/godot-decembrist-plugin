#if TOOLS
using System.Collections.Generic;
using Decembrist.Dock;
using Decembrist.State;
using Decembrist.State.UI.StateMachine;
using Godot;

namespace Decembrist
{
    [Tool]
    public class DecembristPlugin : EditorPlugin
    {
        private Control _decembristDock;
        private List<IDockWrapper> _wrappers = new();
    
        public override void EnablePlugin()
        {
            AddAutoloadSingleton("DecembristAutoload", "res://addons/decembrist_plugin/Autoload/DecembristAutoload.cs");
            CheckSetting(DecembristSettings.ConfigClass, "DecembristConfiguration");
            CheckSetting(DecembristSettings.EventBusEnabled, true);
            CheckSetting(DecembristSettings.LanEventsEnabled, false);
            CheckSetting(DecembristSettings.LanEventsEnabled, false);
        }

        public override void _EnterTree()
        {
            // var texture = GD.Load<Texture>("icon.png");
            AddCustomType(nameof(StateMachine), "Node", StateMachine.Script, null);
            AddCustomType(nameof(State.State), "Node", State.State.Script, null);
            AddCustomType(nameof(Transition), "Node", Transition.Script, null);
            SetUpDecembristDock();
        }

        public override void DisablePlugin()
        {
            RemoveAutoloadSingleton("DecembristAutoload");
            RemoveControlFromBottomPanel(_decembristDock);
            _wrappers.ForEach(wrapper => wrapper.Destructor());
        }

        public override void _ExitTree()
        {
            RemoveCustomType(nameof(StateMachine));
            RemoveCustomType(nameof(State.State));
            RemoveCustomType(nameof(Transition));
        }

        private void CheckSetting(string name, object @default)
        {
            var setting = ProjectSettings.GetSetting(name);
            if (setting == null)
            {
                ProjectSettings.SetSetting(name, @default);
            }
        }

        private void SetUpDecembristDock()
        {
            _decembristDock = DecembristDock.Instance(GetEditorInterface());
            _wrappers.Add(StateMachineController.WrapDock(_decembristDock, this));
            AddControlToBottomPanel(_decembristDock, "Decembrist");
        }
    }
}

#endif