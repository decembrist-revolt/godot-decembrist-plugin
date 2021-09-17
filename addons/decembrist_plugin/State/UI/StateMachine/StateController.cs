#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Decembrist.State.UI.State;
using Decembrist.Utils;
using Godot;

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
        private Button _attachStateScriptButton;
        private Button _resetStateScriptButton;
        private StatePanelController _statePanel;
        private FileDialog _attachStateScriptDialog;

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
            _attachStateScriptButton = _stateMachineGraph.GetNode<Button>("AttachStateScriptButton");
            _attachStateScriptButton.OnButtonPress(() => _attachStateScriptDialog.PopupCentered());
            _resetStateScriptButton = _stateMachineGraph.GetNode<Button>("ResetStateScriptButton");
            _resetStateScriptButton.OnButtonPress(ResetStateScript);
            _attachStateScriptDialog = _stateMachineGraph.GetNode<FileDialog>("AttachStateScriptDialog");
            _attachStateScriptDialog.OnFileSelected(AttachStateScript);

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

        private void ResetStateScript()
        {
            var stateName = _selectedBlocks[0].Name;
            var stateResource = GetStateResource(stateName) ?? throw StateResourceNotFoundEx(stateName);
            stateResource.Script = StateScript.ScriptPath;
            
            RefreshGraph();
            UpdateStateButtons();
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
            var state = new StateResource(stateName!, StateScript.ScriptPath);
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
            var idleStateExists = GetStateResource(StateScript.IdleStateName) != null;
            if (!idleStateExists) throw new Exception($"No {StateScript.IdleStateName} state found");

            _stateBlocks = _currentResource.States
                .Select(CreateStateBlock)
                .Select(SetUpStateBlock)
                .ToDictionary(stateBlock => stateBlock.Name);
            _selectedBlocks.Clear();
            _stateMachineGraph.SetSelected(null);
            DrawTransitionLines();
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
            UpdateStateButtons();
        }

        private void OnNodeSelected(Node node)
        {
            _selectedBlocks = _stateBlocks.Values
                .Where(block => block.Selected)
                .Where(block => block.Name != node.Name)
                .ToList();
            _selectedBlocks.Add((node as GraphNode)!);
            UnselectTransition();
            UpdateStateButtons();
        }

        private void UpdateStateButtons()
        {
            _newStateButton.Visible = _selectedBlocks.Count == 0 && _selectedTransaction == null;
            _resetStateScriptButton.Visible = false;
            _attachStateScriptButton.Visible = false;
            var oneSelected = _selectedBlocks.Count == 1;
            if (oneSelected)
            {
                _attachStateScriptButton.Visible = true;
                var stateName = _selectedBlocks[0].Name;
                var stateResource = GetStateResource(stateName) ?? throw StateResourceNotFoundEx(stateName);
                _resetStateScriptButton.Visible = stateResource.Script != StateScript.ScriptPath;
                _renameStateButton.Visible = stateName != StateScript.IdleStateName;
            }

            _connectStateButton.Visible = _selectedBlocks.Count == 2
                                          && !StatesConnected(_selectedBlocks[0], _selectedBlocks[1]);
            UpdateTransitionButtons();
        }

        private void AttachStateScript(string file)
        {
            if (!ResourceLoader.Exists(file)) throw new Exception($"Script {file} not found");

            var resource = ResourceLoader.Load(file);
            if (resource is not CSharpScript script) throw new Exception($"Script {file} is not c# script");

            var state = script.New();
            if (state is not StateScript) throw new Exception($"Script {file} is not {typeof(StateScript).FullName} class");

            var stateName = _selectedBlocks[0].Name;
            var stateResource = GetStateResource(stateName) ?? throw StateResourceNotFoundEx(stateName);

            stateResource.Script = file;
            RefreshGraph();
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