using System.Collections.Generic;
using Decembrist.Utils;
using Godot;

namespace Decembrist.State
{
    [Tool]
    public class State : Node
    {
        public const string IdleStateName = "Idle";

        public static Script Script => GD.Load<Script>("res://addons/decembrist_plugin/State/State.cs");
        
        [Export]
        public Vector2 Position = Vector2.Zero;

        public List<Transition> Transitions => this.GetChildren<Transition>();

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
