using System.Threading.Tasks;
using Decembrist.Di;
using Decembrist.State;
using Godot;

namespace Decembrist.Example.StateMachineTest
{
    public class StateMachineTest : Node2D, ITest
    {
        private const string State1Name = "State1";
        
        [ChildNode] private StateMachine _stateMachine;

        public int TestCounter = 0;

        public override void _Ready()
        {
            this.InjectAll();
        }

        public async Task Test()
        {
            Assertions.AssertEquals(
                _stateMachine.CurrentScript.Name, 
                StateScript.IdleStateName,
                $"Start state is {StateScript.IdleStateName}");
            _stateMachine.Update();
            Assertions.AssertEquals(1, TestCounter, "Counter value == 1");
            _stateMachine.Update();
            Assertions.AssertEquals(
                _stateMachine.CurrentScript.Name, 
                State1Name,
                $"Start state is {StateScript.IdleStateName}");
        }
    }
}