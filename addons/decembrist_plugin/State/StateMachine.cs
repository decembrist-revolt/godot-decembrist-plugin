using System;
using System.Collections.Generic;
using System.Linq;
using Decembrist.Utils;
using Godot;

namespace Decembrist.State
{
    [Tool]
    public class StateMachine : Node
    {
        public static Script Script => GD.Load<Script>("res://addons/decembrist_plugin/State/StateMachine.cs");

        public List<StateScript> States => this.GetChildren<StateScript>();

        public bool EnableTrace = false;
        public bool TraceNewStateOnly = false;
        public int MaxTransitionCount = 100;

        private Dictionary<string, StateScript> _stateMap;

        public StateScript CurrentScript { private set; get; }

        public override void _Ready()
        {
            var idle = States.FirstOrDefault(state => state.Name == StateScript.IdleStateName);
            CurrentScript = idle ?? throw new Exception($"{StateScript.IdleStateName} state not found");
            CurrentScript.OnEnter(GetParent());
            _stateMap = States.ToDictionary(state => state.Name);
        }

        public void Update()
        {
            // PrintCurrentState();
            // var transitions = CurrentScript.Transitions;
            // var sceneRoot = GetParent();
            // var trueTransition = transitions.FirstOrDefault(transition => transition.ShouldTransit(sceneRoot));
            // var transitionCount = 0;
            // while (trueTransition != null)
            // {
            //     if (MaxTransitionCount < ++transitionCount) throw new Exception("Max transition count exceeded");
            //     
            //     TraceMessage($"{LogPrefix} UPDATE: true transition is {trueTransition.Name}");
            //     CurrentScript.OnExit(sceneRoot);
            //     var targetState = trueTransition.Target;
            //     if (!_stateMap.ContainsKey(targetState)) throw new Exception($"StateScript not found {targetState}");
            //     var state = _stateMap[targetState];
            //     CurrentScript = state;
            //     PrintCurrentState();
            //     CurrentScript.OnEnter(sceneRoot);
            //     trueTransition = transitions.FirstOrDefault(transition => transition.ShouldTransit(sceneRoot));
            // }
            //
            // TraceMessage($"{LogPrefix} UPDATE: state tick {CurrentScript.Name}");
            // CurrentScript.OnTick(sceneRoot);
        }

        private string LogPrefix => $"StateMachine {Name}: ";

        private void PrintCurrentState()
        {
            if (EnableTrace) GD.Print($"{LogPrefix} UPDATE: current state is {CurrentScript.Name}");
        }

        private void TraceMessage(string message)
        {
            if (EnableTrace && !TraceNewStateOnly) GD.Print(message);
        }
    }
}