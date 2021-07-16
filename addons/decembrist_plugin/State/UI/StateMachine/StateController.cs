#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Decembrist.State.UI.State;
using Decembrist.Utils;
using Godot;
using StateData = Decembrist.State.State;

namespace Decembrist.State.UI.StateMachine
{
    public partial class StateMachineController
    {
        private const string NewStateLabelText = "New state";
        private const string RenameStateLabelText = "State name";
        private const string AddStateButtonText = "Add";
        private const string RenameStateButtonText = "Rename";

        private Button _newStateButton;
        private Button _connectStateButton;
        private Button _renameStateButton;
        private StatePanelController _statePanel;

        private List<GraphNode> _selectedBlocks = new();
        private Dictionary<string, GraphNode> _stateBlocks = new();

        private void InitStateController()
        {
            var statePanel = _stateMachineGraph.GetNode<PopupPanel>("StatePanel");
            _statePanel = new StatePanelController(statePanel);

            _newStateButton = _stateMachineGraph.GetNode<Button>("NewStateButton");
            _newStateButton.OnButtonPress(ShowNewStatePanel);
            _connectStateButton = _stateMachineGraph.GetNode<Button>("ConnectStateButton");
            _connectStateButton.OnButtonPress(ConnectStates);
            _renameStateButton = _stateMachineGraph.GetNode<Button>("RenameStateButton");
            _renameStateButton.OnButtonPress(ShowRenameStatePanel);

            _stateMachineGraph.OnNodeSelected(OnNodeSelected);
            _stateMachineGraph.OnNodeUnselected(OnNodeUnselected);
        }

        private StateResource? GetStateResource(string stateName) => _currentResource?
            .States.FirstOrDefault(state => state.Name == stateName);

        private void RefreshStates()
        {
            _stateMachineGraph.SetSelected(null);
            _renameStateButton.Visible = false;
            _connectStateButton.Visible = false;
            if (_currentResource != null) _newStateButton.Visible = true;
        }

        private void ShowNewStatePanel()
        {
            if (_currentResource == null) throw EmptyResourceEx();

            _statePanel.PopupStateEdit(
                NewStateLabelText, "", AddStateButtonText, SaveState, ValidateStateName);
        }

        private void ShowRenameStatePanel()
        {
            if (_currentResource == null) throw EmptyResourceEx();
            if (_selectedBlocks.Count != 1) throw WrongSelectedItemsCountEx();

            var selectedBlockName = _selectedBlocks[0].Name;
            void SetBlockName(string newName) => RenameState(selectedBlockName, newName);
            _statePanel.PopupStateEdit(
                RenameStateLabelText, selectedBlockName, RenameStateButtonText, SetBlockName, ValidateStateName);
        }

        private void SaveState(string? stateName)
        {
            var state = new StateResource(stateName!);
            _currentResource!.States.Add(state);
            RefreshGraph();
        }

        private void RenameState(string oldName, string? stateName)
        {
            var stateBlock = _stateBlocks[oldName];
            stateBlock.Name = stateName!;
            RefreshGraph();
        }

        private bool ValidateStateName(string? stateName)
        {
            if (_currentResource == null) throw EmptyResourceEx();

            var valid = !string.IsNullOrEmpty(stateName);
            valid &= GetStateResource(stateName!) == null;
            return valid;
        }

        private void DrawStates()
        {
            if (_currentResource == null) throw EmptyResourceEx();

            ClearStates();
            var idleStateExists = GetStateResource(StateData.IdleStateName) != null;
            if (!idleStateExists) throw new Exception($"No {StateData.IdleStateName} state found");

            _stateBlocks = _currentResource.States
                .Select(CreateStateBlock)
                .Select(SetUpStateBlock)
                .ToDictionary(stateBlock => stateBlock.Name);
            _selectedBlocks.Clear();
            _stateMachineGraph.SetSelected(null);
        }

        private void ClearStates()
        {
            _stateBlocks.Values.ToList().ForEach(_stateMachineGraph.RemoveChild);
            _stateBlocks.Clear();
        }

        private void DeleteState(string stateName)
        {
            if (GetStateResource(stateName) is { } stateResource)
            {
                _currentResource!.States.Remove(stateResource);
            }

            RefreshGraph();
        }

        private void OnNodeUnselected(Node node)
        {
            _selectedBlocks.Remove((node as GraphNode)!);
            OnChangeSelectedNode();
        }

        private void OnNodeSelected(Node node)
        {
            _selectedBlocks = _stateBlocks.Values
                .Where(block => block.Selected)
                .Where(block => block.Name != node.Name)
                .ToList();
            _selectedBlocks.Add((node as GraphNode)!);
            OnChangeSelectedNode();
            // _selectedTransaction?.SetSelected(false);
            // if (anySelected && node == null) return;

            // _currentState = node != null ? _stateBlocks[node.Name] : null;
            // _connectStateButton.Visible = node != null && _stateBlocks.Count > 1;
            // _newStateButton.Visible = node == null;
        }

        private void OnChangeSelectedNode()
        {
            _newStateButton.Visible = _selectedBlocks.Count == 0;
            _renameStateButton.Visible = _selectedBlocks.Count == 1;
            _connectStateButton.Visible = _selectedBlocks.Count == 2;
        }

        private void OnMoveStateBlock(GraphNode stateBlock)
        {
            if (GetStateResource(stateBlock.Name) is not { } stateResource)
            {
                throw new Exception("Empty state resource");
            }

            stateResource.Position = stateBlock.Offset;
        }
        
        private Exception StateResourceNotFoundEx(string stateName) => new($"State resource {stateName} not found!");
    }
}