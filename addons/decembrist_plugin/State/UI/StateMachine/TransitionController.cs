#nullable enable
using System;
using System.Linq;
using Decembrist.State.UI.Transition;
using TransitionData = Decembrist.State.Transition;

namespace Decembrist.State.UI.StateMachine
{
    public partial class StateMachineController
    {
        private TransitionLine _selectedTransaction;

        /// <summary>
        /// Creates transition between selected state and selected
        /// </summary>
        private void ConnectStates()
        {
            if (_currentResource == null) throw EmptyResourceEx();
            if (_selectedBlocks.Count != 2) throw WrongSelectedItemsCountEx();

            var fromBlock = _selectedBlocks[0];
            var toBlock = _selectedBlocks[1];
            var stateResource = _currentResource!.States.FirstOrDefault(state => state.Name == fromBlock.Name);
            if (stateResource == null) throw new Exception($"state resource with name {fromBlock.Name} not found");
            var transition = new TransitionResource(toBlock.Name);
            stateResource.Transitions.Add(transition);
            
            RefreshGraph();
        }

        private void SetUpTransitionUi(TransitionLine transitionLine)
        {
            // transitionLine.Subscribe(nameof(TransitionLine.Select), (TransitionData transition) =>
            // {
            //     _selectedTransaction?.SetSelected(false);
            //     transitionLine.SetSelected(true);
            //     _selectedTransaction = transitionLine;
            //     _plugin.GetEditorInterface().GetSelection().Clear();
            //     _plugin.GetEditorInterface().GetSelection().AddNode(transition);
            //     // _plugin.GetEditorInterface().EditNode(transition);
            //     _stateMachineGraph.SetSelected(null);
            //     OnNodeUnSelected(_currentState?.StateUi);
            // });
        }
    }
}