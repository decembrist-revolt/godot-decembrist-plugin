#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;
using TransitionData = Decembrist.State.Transition;
using StateData = Decembrist.State.State;

namespace Decembrist.State
{
    public class StateMachineResource
    {
        private const string StateMachineDataProp = "state_machine_data";
        private const string StateMachineResourceGd = "res://addons/decembrist_plugin/State/state_machine_resource.gd";
        private const string StatesKey = "states";

        public readonly Resource Resource;
        public readonly List<StateResource> States;

        public StateMachineResource(Resource stateMachineGd)
        {
            Resource = stateMachineGd;
            var value = stateMachineGd.Get(StateMachineDataProp)
                        ?? new Dictionary {{StatesKey, CreateNewStates()}};
            if (value is not Dictionary dictionary)
            {
                throw new Exception($"{stateMachineGd.ResourcePath} has wrong format!");
            }

            if (dictionary[StatesKey] is not Array states
                || !states.OfType<object>().All(state => state is Dictionary))
            {
                throw new Exception($"{stateMachineGd.ResourcePath} [states] has wrong format!");
            }

            States = states.OfType<Dictionary>()
                .Select(state => new StateResource(state))
                .ToList();
        }

        public StateMachineResource(List<StateResource> states)
        {
            States = states;
        }

        public void SaveResource()
        {
            Resource.Set(StateMachineDataProp, AsDictionary());
            ResourceSaver.Save(Resource.ResourcePath, Resource);
            Resource.EmitChanged();
        }

        public Dictionary AsDictionary()
        {
            var states = States.Select(state => state.AsDictionary());
            return new Dictionary
            {
                {StatesKey, new Array(states)}
            };
        }

        private Array CreateNewStates() => new()
        {
            new StateResource(StateData.IdleStateName, StateData.Script.ResourcePath).AsDictionary()
        };
    }

    public class StateResource
    {
        private const string NameKey = "name";
        private const string ScriptKey = "script";
        private const string PositionKey = "position";
        private const string TransitionsKey = "transitions";

        public string Name;
        public string? Script;
        public Vector2 Position;
        public List<TransitionResource> Transitions;

        public StateResource(Dictionary state)
        {
            if (state[NameKey] is not string name
                || state[ScriptKey] is not string script
                || state[PositionKey] is not Vector2 position)
            {
                throw new Exception($"State {JSON.Print(state)} has wrong format!");
            }

            Name = name;
            Script = script;
            Position = position;

            if (state[TransitionsKey] is not Array transitions
                || !transitions.OfType<object>().All(transition => transition is Dictionary))
            {
                throw new Exception($"State {JSON.Print(state)} [transitions] has wrong format!");
            }

            Transitions = transitions.OfType<Dictionary>()
                .Select(transition => new TransitionResource(transition))
                .ToList();
        }

        public StateResource(
            string name,
            string? script = null,
            Vector2? position = null,
            IEnumerable<TransitionResource>? transitions = null)
        {
            Name = name;
            Script = script ?? StateData.Script.ResourcePath;
            Position = position ?? Vector2.Zero;
            Transitions = (transitions ?? Enumerable.Empty<TransitionResource>()).ToList();
        }

        public Dictionary AsDictionary()
        {
            var transitions = Transitions.Select(transition => transition.AsDictionary());
            return new Dictionary
            {
                {NameKey, Name},
                {ScriptKey, Script},
                {PositionKey, Position},
                {TransitionsKey, new Array(transitions)},
            };
        }
    }

    public class TransitionResource
    {
        private const string TargetKey = "target";
        private const string ScriptKey = "script";
        
        public readonly string Target;
        public readonly string? Script;

        public TransitionResource(Dictionary transition)
        {
            if (transition[TargetKey] is not string target || transition[ScriptKey] is not string script)
            {
                throw new Exception($"Transition {JSON.Print(transition)} has wrong format!");
            }
            
            Target = target;
            Script = script;
        }

        public TransitionResource(string target, string? script = null)
        {
            Target = target;
            Script = script;
        }

        public Dictionary AsDictionary() => new()
        {
            {TargetKey, Target},
            {ScriptKey, Script},
        };
    }
}