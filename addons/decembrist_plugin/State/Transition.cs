using Godot;

namespace Decembrist.State
{
    [Tool]
    public class Transition: Node
    {
        public static Script Script => GD.Load<Script>("res://addons/decembrist_plugin/State/Transition.cs");

        public virtual bool ShouldTransit(Node sceneRoot) => false;
    }
}