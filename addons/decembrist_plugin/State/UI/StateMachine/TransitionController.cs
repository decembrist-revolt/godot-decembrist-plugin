#nullable enable
using System;
using System.Linq;
using Decembrist.State.UI.Transition;
using Decembrist.Utils;
using Decembrist.Utils.Callback;
using Godot;

namespace Decembrist.State.UI.StateMachine
{
    public partial class StateMachineController
    {
        private Button _attachTransitionScriptButton;
        private Button _resetTransitionScriptButton;
        private FileDialog _attachTransitionScriptDialog;

        private TransitionLine? _selectedTransaction;

        private void InitTransitionController()
        {
            _stateMachineGraph.OnMouseInput(
                _ => GraphClick(),
                @event => @event is {ButtonIndex: (int) ButtonList.Left, Pressed: true}
            );
            _attachTransitionScriptButton = _stateMachineGraph.GetNode<Button>("AttachTransitionScriptButton");
            _attachTransitionScriptButton.OnButtonPress(() => _attachTransitionScriptDialog.PopupCentered());
            _resetTransitionScriptButton = _stateMachineGraph.GetNode<Button>("ResetTransitionScriptButton");
            _resetTransitionScriptButton.OnButtonPress(ResetTransitionScript);
            _attachTransitionScriptDialog = _stateMachineGraph.GetNode<FileDialog>("AttachTransitionScriptDialog");
            _attachTransitionScriptDialog.OnFileSelected(AttachTransitionScript);
        }

        private void ResetTransitionScript()
        {
            if (_selectedTransaction == null) return;

            _selectedTransaction.Resource.Script = TransitionScript.ScriptPath;
            
            RefreshGraph();
            UnselectTransition();
            UpdateStateButtons();
        }

        private void AttachTransitionScript(string file)
        {
            if (_selectedTransaction == null) throw new Exception("Transition not selected");
            if (!ResourceLoader.Exists(file)) throw new Exception($"Script {file} not found");

            var resource = ResourceLoader.Load(file);
            if (resource is not CSharpScript script) throw new Exception($"Script {file} is not c# script");

            var state = script.New();
            if (state is not TransitionScript)
                throw new Exception(
                    $"Script {file} is not {typeof(TransitionScript).FullName} class");

            _selectedTransaction.Resource.Script = file;

            RefreshGraph();
        }

        private void GraphClick()
        {
            if (_currentResource == null) return;

            UnselectTransition();
            UpdateStateButtons();
        }

        /// <summary>
        /// Creates transition between selected state and selected
        /// </summary>
        private void ConnectStates()
        {
            if (_currentResource == null) throw EmptyResourceEx();
            if (_selectedBlocks.Count != 2) throw WrongSelectedItemsCountEx();

            var fromBlock = _selectedBlocks[0];
            var toBlock = _selectedBlocks[1];
            if (StatesConnected(fromBlock, toBlock))
            {
                throw new Exception($"States [{fromBlock.Name}]->[{toBlock.Name}] connected already!");
            }

            var stateResource = GetStateResource(fromBlock.Name) ?? throw StateResourceNotFoundEx(fromBlock.Name);
            var transition = new TransitionResource(toBlock.Name, TransitionScript.Script.ResourcePath);
            stateResource.Transitions.Add(transition);

            RefreshGraph();
        }

        private void DrawTransitionLines()
        {
            foreach (var (stateName, block) in _stateBlocks)
            {
                var stateResource = GetStateResource(stateName) ?? throw StateResourceNotFoundEx(stateName);
                stateResource.Transitions
                    .Select(transition => new TransitionLine(block, transition))
                    .ToList()
                    .ForEach(line =>
                    {
                        SetUpTransitionUi(line);
                        block.AddChild(line);
                    });
            }
        }

        private void SetUpTransitionUi(TransitionLine transitionLine)
        {
            transitionLine.Subscribe(nameof(TransitionLine.Select), () =>
            {
                UnselectTransition();
                transitionLine.SetSelected(true);
                _selectedTransaction = transitionLine;
                _stateMachineGraph.SetSelected(null);
                _selectedBlocks.Clear();
                UpdateStateButtons();
            });
        }

        private void UpdateTransitionButtons()
        {
            var transitionSelected = _selectedTransaction != null;
            _attachTransitionScriptButton.Visible = transitionSelected;
            _resetTransitionScriptButton.Visible = false;
            if (transitionSelected)
            {
                var isDefaultScript = _selectedTransaction.Resource.Script == TransitionScript.ScriptPath;
                _resetTransitionScriptButton.Visible = transitionSelected && !isDefaultScript;
            }
        }

        private void UnselectTransition()
        {
            _selectedTransaction?.SetSelected(false);
            _selectedTransaction = null;
        }

        private bool StatesConnected(GraphNode from, GraphNode to)
        {
            var stateResource = GetStateResource(from.Name) ?? throw StateResourceNotFoundEx(from.Name);
            return stateResource.Transitions.Any(transition => transition.Target == to.Name);
        }
    }
}