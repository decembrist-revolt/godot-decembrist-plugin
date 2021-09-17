using System;
using Decembrist.State;
using Godot;

namespace Decembrist.Example.StateMachineTest
{
    [Tool]
    public class IdleState : StateScript
    {
        public override void OnEnter(Node sceneRoot)
        {
            if (sceneRoot is not StateMachineTest test) throw new Exception("Wrong scene root");
            test.TestCounter++;
        }

        public override void OnTick(Node sceneRoot)
        {
            if (sceneRoot is not StateMachineTest test) throw new Exception("Wrong scene root");
            test.TestCounter++;
        }

        public override void OnExit(Node sceneRoot)
        {
            if (sceneRoot is not StateMachineTest test) throw new Exception("Wrong scene root");
            test.TestCounter--;
        }
    }
}
