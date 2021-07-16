#nullable enable
using System;
using System.Collections.Generic;
using Decembrist.Dock;
using Decembrist.Utils;
using Godot;
using StateMachineComponent = Decembrist.State.StateMachine;

namespace Decembrist.State.UI.StateMachine
{
    [Tool]
    public partial class StateMachineController : IDockWrapper
    {
        private readonly OptionButton _machinesComboButton;
        private readonly Label _noStateMachineLabel;
        private readonly Button _refreshButton;
        private readonly Button _newMachineButton;
        private readonly Button _saveMachineButton;
        private readonly Button _closeMachineButton;
        private readonly Button _openMachineButton;
        private readonly GraphEdit _stateMachineGraph;
        private readonly FileDialog _newMachineDialog;
        private readonly FileDialog _loadMachineDialog;

        private readonly List<Action> _unsubscribes = new();
        private readonly EditorPlugin _plugin;

        private List<StateMachineComponent> _stateMachines;
        private StateMachineResource? _currentResource;

        public static StateMachine.StateMachineController WrapDock(
            Control decembristDock,
            EditorPlugin plugin) => new(decembristDock, plugin);

        private StateMachineController(Control decembristDock, EditorPlugin plugin)
        {
            _plugin = plugin;
            _stateMachineGraph = decembristDock.GetNode<GraphEdit>("TabContainer/StateMachine");
            // Dialogs
            _newMachineDialog = _stateMachineGraph.GetNode<FileDialog>("NewMachineDialog");
            _newMachineDialog.OnFileSelected(LoadResource);
            _loadMachineDialog = _stateMachineGraph.GetNode<FileDialog>("LoadMachineDialog");
            _loadMachineDialog.OnFileSelected(LoadResource);
            // Machine buttons
            _newMachineButton = _stateMachineGraph.GetNode<Button>("NewMachineButton");
            _newMachineButton.OnButtonPress(() => _newMachineDialog.PopupCentered());
            _openMachineButton = _stateMachineGraph.GetNode<Button>("OpenMachineButton");
            _openMachineButton.OnButtonPress(() => _loadMachineDialog.PopupCentered());
            _saveMachineButton = _stateMachineGraph.GetNode<Button>("SaveMachineButton");
            _saveMachineButton.OnButtonPress(SaveResource);
            _closeMachineButton = _stateMachineGraph.GetNode<Button>("CloseMachineButton");
            _closeMachineButton.OnButtonPress(CloseResource);
            _refreshButton = _stateMachineGraph.GetNode<Button>("RefreshButton");
            _refreshButton.OnButtonPress(RefreshGraph);
            _connectStateButton = _stateMachineGraph.GetNode<Button>("ConnectStateButton");
            _connectStateButton.OnButtonPress(ConnectStates);
            // Info
            _noStateMachineLabel = _stateMachineGraph.GetNode<Label>("NoStateMachineLabel");
            InitStateController();
        }

        private void LoadResource(string file)
        {
            var resource = ResourceLoader.Exists(file) ? ResourceLoader.Load(file) : new Resource {ResourcePath = file};

            _currentResource = new StateMachineResource(resource);
            _saveMachineButton.Visible = true;
            _closeMachineButton.Visible = true;
            _newMachineButton.Visible = false;
            _openMachineButton.Visible = false;
            RefreshGraph();
        }
        
        private void CloseResource()
        {
            _currentResource = null;
            _saveMachineButton.Visible = false;
            _closeMachineButton.Visible = false;
            _newMachineButton.Visible = true;
            _openMachineButton.Visible = true;
            _newStateButton.Visible = false;
            _renameStateButton.Visible = false;
            _connectStateButton.Visible = false;
            RefreshGraph();
        }

        private void SaveResource()
        {
            if (_currentResource == null) throw EmptyResourceEx();
            
            _currentResource!.SaveResource();
        }

        public void Destructor()
        {
            foreach (var unsubscribe in _unsubscribes)
            {
                unsubscribe();
            }
        }

        private void RefreshGraph()
        {
            if (_currentResource != null)
            {
                // _newStateButton.Visible = true;
                _noStateMachineLabel.Visible = false;
                _saveMachineButton.Visible = true;
                _closeMachineButton.Visible = true;
                DrawStates();
            }
            else
            {
                _noStateMachineLabel.Visible = true;
                _saveMachineButton.Visible = false;
                _closeMachineButton.Visible = false;
                ClearStates();
            }

            RefreshStates();
        }

        private Exception EmptyResourceEx() => new("Empty state machine error");

    }
}