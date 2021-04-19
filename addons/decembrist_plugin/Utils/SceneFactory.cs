using Godot;

namespace Decembrist.Utils
{
    public static class SceneFactory
    {
        public static T GetInstance<T>(SceneInfo<T> info) where T : Node
        {
            var scene = (PackedScene) ResourceLoader.Load(info.ResourcePath);
            return (T) scene.Instance();
        }
    }
    
    public class SceneInfo<T> where T : Node
    {
        public readonly string ResourcePath;
        public SceneInfo(string resourcePath)
        {
            ResourcePath = resourcePath;
        }
    }
}