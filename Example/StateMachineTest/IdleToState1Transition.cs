using System;
using Decembrist.State;
using Godot;

namespace Decembrist.Example.StateMachineTest
{
    public class IdleToState1Transition : TransitionScript
    {
        public override bool ShouldTransit(Node sceneRoot)
        {
            if (sceneRoot is not StateMachineTest test) throw new Exception("Wrong scene root");
            return test.TestCounter == 2;
        }
    }
}
