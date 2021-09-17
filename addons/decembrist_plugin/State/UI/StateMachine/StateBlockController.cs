using System;
using System.Linq;
using Decembrist.Utils;
using Godot;

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
            stateBlock.ShowClose = stateBlock.Name != StateScript.IdleStateName;
            var stateScript = stateResource.Script;
            if (stateScript != null)
            {
                stateBlock.Title = stateScript.Split("/").Last();
                stateBlock.HintTooltip = stateScript;
            }
            return stateBlock;
        }
        
        private GraphNode SetUpStateBlock(GraphNode stateBlock)
        {
            _stateMachineGraph.AddChild(stateBlock);
            stateBlock.OnOffsetChanged(() => OnMoveStateBlock(stateBlock));
            stateBlock.OnCloseRequest(() => DeleteState(stateBlock.Name));
            return stateBlock;
        }

        private Exception WrongSelectedItemsCountEx() => new("Wrong selected items count");
    }
}