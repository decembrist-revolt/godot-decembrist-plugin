using System.Collections.Generic;
using Decembrist.Utils;
using Godot;

namespace Decembrist.State
{
    [Tool]
    public class StateScript : Node
    {
        public const string IdleStateName = "Idle";
        public const string ScriptPath = "res://addons/decembrist_plugin/State/StateScript.cs";
        
        public static Script Script => GD.Load<Script>(ScriptPath);
        
        [Export]
        public Vector2 Position = Vector2.Zero;

        public List<TransitionScript> Transitions => this.GetChildren<TransitionScript>();

        public virtual void OnEnter(Node sceneRoot)
        {
        }
        
        public virtual void OnTick(Node sceneRoot)
        {
        }
        
        public virtual void OnExit(Node sceneRoot)
        {
        }
    }
}
