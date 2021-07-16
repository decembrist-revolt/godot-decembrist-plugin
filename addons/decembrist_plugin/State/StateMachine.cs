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

        public List<State> States => this.GetChildren<State>();

        public bool EnableTrace = false;
        public bool TraceNewStateOnly = false;
        public int MaxTransitionCount = 100;

        private Dictionary<string, State> _stateMap;

        public State CurrentState { private set; get; }

        public override void _Ready()
        {
            var idle = States.FirstOrDefault(state => state.Name == State.IdleStateName);
            CurrentState = idle ?? throw new Exception($"{State.IdleStateName} state not found");
            CurrentState.OnEnter(GetParent());
            _stateMap = States.ToDictionary(state => state.Name);
        }

        public void Update()
        {
            PrintCurrentState();
            var transitions = CurrentState.Transitions;
            var sceneRoot = GetParent();
            var trueTransition = transitions.FirstOrDefault(transition => transition.ShouldTransit(sceneRoot));
            var transitionCount = 0;
            while (trueTransition != null)
            {
                if (MaxTransitionCount < ++transitionCount) throw new Exception("Max transition count exceeded");
                
                TraceMessage($"{LogPrefix} UPDATE: true transition is {trueTransition.Name}");
                CurrentState.OnExit(sceneRoot);
                var targetState = trueTransition.Target;
                if (!_stateMap.ContainsKey(targetState)) throw new Exception($"State not found {targetState}");
                var state = _stateMap[targetState];
                CurrentState = state;
                PrintCurrentState();
                CurrentState.OnEnter(sceneRoot);
                trueTransition = transitions.FirstOrDefault(transition => transition.ShouldTransit(sceneRoot));
            }

            TraceMessage($"{LogPrefix} UPDATE: state tick {CurrentState.Name}");
            CurrentState.OnTick(sceneRoot);
        }

        private string LogPrefix => $"StateMachine {Name}: ";

        private void PrintCurrentState()
        {
            if (EnableTrace) GD.Print($"{LogPrefix} UPDATE: current state is {CurrentState.Name}");
        }

        private void TraceMessage(string message)
        {
            if (EnableTrace && !TraceNewStateOnly) GD.Print(message);
        }
    }
}