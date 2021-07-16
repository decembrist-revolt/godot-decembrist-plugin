using System;
using Decembrist.State.UI.Transition;
using Decembrist.Utils;
using Godot;
using StateData = Decembrist.State.State;

namespace Decembrist.State.UI.StateMachine
{
    public partial class StateMachineController
    {
        public static readonly SceneInfo<GraphNode> SceneInfo =
            new("res://addons/decembrist_plugin/State/UI/State/StateBlock.tscn");

        private GraphNode CreateStateBlock(StateResource stateResource)
        {
            var stateBlock = SceneInfo.GetInstance();
            stateBlock.Name = stateResource.Name;
            stateBlock.Offset = stateResource.Position;
            stateBlock.GetNode<Label>("Label").Text = stateBlock.Name;
            stateBlock.ShowClose = stateBlock.Name != StateData.IdleStateName;
            return stateBlock;
            // StateUi = node;
            // State = state;
            // Name = state.Name;
            //
            // _label = StateUi.GetNode<Label>("Label");
            // _label.Text = Name;
            //
            // StateUi.Offset = state.Position;
            // StateUi.ShowClose = Name != Decembrist.State.State.IdleStateName;
            //
            // StateUi.OnOffsetChanged(OnNodeMove);
            //
            // State.GetChildren<TransitionLine>().ForEach(State.RemoveChild);
            //
            // State.Transitions
            //     .Select(transition => new TransitionLine(node, transition))
            //     .ToList()
            //     .ForEach(transitionUi => StateUi.AddChild(transitionUi));
        }
        
        private GraphNode SetUpStateBlock(GraphNode stateBlock)
        {
            _stateMachineGraph.AddChild(stateBlock);
            stateBlock.OnOffsetChanged(() => OnMoveStateBlock(stateBlock));
            stateBlock.GetChildren<TransitionLine>().ForEach(SetUpTransitionUi);
            stateBlock.OnCloseRequest(() => DeleteState(stateBlock.Name));
            return stateBlock;
        }

        private Exception WrongSelectedItemsCountEx() => new("Wrong selected items count");
    }
}