#if TOOLS
using Godot;

[Tool]
public class DecembristPlugin : EditorPlugin
{
    public override void EnablePlugin()
    {
        AddAutoloadSingleton("DecembristAutoload", "res://addons/decembrist_plugin/Autoload/DecembristAutoload.cs");
        CheckSetting(DecembristSettings.ConfigClass, "DecembristConfiguration");
        CheckSetting(DecembristSettings.EventBusEnabled, true);
        CheckSetting(DecembristSettings.LanEventsEnabled, false);
    }

    public override void DisablePlugin()
    {
        RemoveAutoloadSingleton("DecembristAutoload");
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

#endif