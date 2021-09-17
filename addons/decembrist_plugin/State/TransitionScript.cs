using Godot;

namespace Decembrist.State
{
    [Tool]
    public class TransitionScript: Node
    {
        public const string ScriptPath = "res://addons/decembrist_plugin/State/TransitionScript.cs";
        
        public static Script Script => GD.Load<Script>(ScriptPath);

        public virtual bool ShouldTransit(Node sceneRoot) => false;
    }
}