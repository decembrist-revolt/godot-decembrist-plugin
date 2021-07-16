using System.Threading.Tasks;
using Decembrist.Di;
using Decembrist.State;
using Godot;
using StateData = Decembrist.State.State;

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
                _stateMachine.CurrentState.Name, 
                StateData.IdleStateName,
                $"Start state is {StateData.IdleStateName}");
            _stateMachine.Update();
            Assertions.AssertEquals(1, TestCounter, "Counter value == 1");
            _stateMachine.Update();
            Assertions.AssertEquals(
                _stateMachine.CurrentState.Name, 
                State1Name,
                $"Start state is {StateData.IdleStateName}");
        }
    }
}