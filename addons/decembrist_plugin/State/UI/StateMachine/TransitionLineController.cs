using System.Linq;
using Decembrist.State.UI.Transition;
using Godot;

namespace Decembrist.State.UI.StateMachine
{
    public partial class StateMachineController
    {
        private void CreateTransitionLines(GraphNode stateBlock)
        {
            var stateName = stateBlock.Name;
            var stateResource = GetStateResource(stateName) ?? throw StateResourceNotFoundEx(stateName);
            stateResource.Transitions
                .Select(transition => new TransitionLine(stateBlock, transition))
                .ToList()
                .ForEach(line => stateBlock.AddChild(line));
        }
    }
}