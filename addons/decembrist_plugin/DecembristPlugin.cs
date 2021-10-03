#if TOOLS

using Godot;

namespace Decembrist
{
    [Tool]
    public class DecembristPlugin : EditorPlugin
    {
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
        }

        public override void DisablePlugin()
        {
            RemoveAutoloadSingleton("DecembristAutoload");
        }

        public override void _ExitTree()
        {
        }

        private void CheckSetting(string name, object @default)
        {
            var setting = ProjectSettings.GetSetting(name);
            if (setting == null)
            {
                ProjectSettings.SetSetting(name, @default);
            }
        }
    }
}

#endif