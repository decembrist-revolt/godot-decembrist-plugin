#if TOOLS
using Godot;

[Tool]
public class DecembristPlugin : EditorPlugin
{
    public override void EnablePlugin()
    {
        AddAutoloadSingleton("DI", "res://addons/decembrist_plugin/Autoload/DiService.cs");
        CheckSetting("decembrist/config_class", "DecembristConfiguration");
    }

    public override void DisablePlugin()
    {
        RemoveAutoloadSingleton("DI");
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