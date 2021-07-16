using Decembrist.Utils;
using Godot;

namespace Decembrist.Dock
{
    public static class DecembristDock
    {
        public static readonly SceneInfo<Control> SceneInfo =
            new("res://addons/decembrist_plugin/Dock/DecembristDock.tscn");

        public static Control Instance(EditorInterface editorInterface)
        {
            var dock = SceneInfo.GetInstance();
            return dock;
        }
    }
}