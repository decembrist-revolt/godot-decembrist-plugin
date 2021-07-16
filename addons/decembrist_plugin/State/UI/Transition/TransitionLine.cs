using System;
using Decembrist.Utils;
using Godot;
using TransitionData = Decembrist.State.Transition;
using StateData = Decembrist.State.State;
using StateMachineData = Decembrist.State.StateMachine;

namespace Decembrist.State.UI.Transition
{
    public class TransitionLine : Node2D
    {
        public const string NamePrefix = "TransitionTo";
        
        public const string LineNodeName = "TransitionLine";
        public const string ClickAreaNodeName = "ClickArea";
        public const float LineShift = 70;
        public const float ArrowLength = 20f;

        [Signal]
        public delegate void Select(TransitionData transition);

        private readonly GraphNode _parent;
        private readonly TransitionResource _transition;
        private bool _selected = false;

        private Line2D Line => GetNode<Line2D>(LineNodeName);
        private Control ClickArea => GetNode<Control>(ClickAreaNodeName);

        public TransitionLine(GraphNode parent, TransitionResource transition)
        {
            _parent = parent;
            _transition = transition;
            ShowBehindParent = true;
            Name = NamePrefix + transition.Target;
        }

        public override void _Draw()
        {
            AttachTransitionLine();
            AttachClickArea();
        }

        public override void _Process(float delta)
        {
            var target = GetTargetPoint();
            Line?.SetPointPosition(0, target.Normalized() * LineShift);
            Line?.SetPointPosition(1, target);
            if (target.Length() > 240)
            {
                DrawArrow(target);
            }
            else
            {
                Line?.SetPointPosition(2, target);
                Line?.SetPointPosition(3, target);
                Line?.SetPointPosition(4, target);
                Line?.SetPointPosition(5, target);
                ClickArea.Visible = false;
            }
        }

        public void SetSelected(bool selected) => _selected = selected;

        private void AttachClickArea()
        {
            var clickArea = new Panel
            {
                Name = ClickAreaNodeName,
                RectSize = new Vector2(60, 60),
                SelfModulate = Colors.Transparent,
            };
            AddChild(clickArea);

            clickArea.OnMouseInput(
                _ => EmitSignal(nameof(Select), _transition),
                @event => @event is {ButtonIndex: (int) ButtonList.Left, Pressed: true}
            );
        }

        private void AttachTransitionLine()
        {
            var offset = _parent.RectPivotOffset;
            var target = GetTargetPoint();

            var line = new Line2D
            {
                Name = LineNodeName,
                Position = offset,
                DefaultColor = Colors.White,
                Width = 6,
            };

            line.AddPoint(target.Normalized() * 100);
            line.AddPoint(Vector2.Zero);
            line.AddPoint(Vector2.Zero);
            line.AddPoint(Vector2.Zero);
            line.AddPoint(Vector2.Zero);
            line.AddPoint(Vector2.Zero);
            AddChild(line);
        }

        private void DrawArrow(Vector2 target)
        {
            var halfPoint = target * new Vector2(0.85f, 0.85f);
            if (Line != null)
            {
                Line.SetPointPosition(2, halfPoint);
                Line.DefaultColor = _selected ? new Color(Colors.Cyan, 0.5f) : new Color(Colors.White, 0.5f);
                Line.ShowOnTop = _selected;
                var arrowWingX = halfPoint.x - (halfPoint.x + halfPoint.y) / halfPoint.Length() * ArrowLength;
                var arrowWingY = halfPoint.y - (-halfPoint.x + halfPoint.y) / halfPoint.Length() * ArrowLength;
                Line.SetPointPosition(3, new Vector2(arrowWingX, arrowWingY));
                Line.SetPointPosition(4, new Vector2(arrowWingX, arrowWingY).Reflect(halfPoint.Normalized()));
                Line.SetPointPosition(5, halfPoint);
            }

            ClickArea.Visible = true;
            ClickArea.ShowOnTop = true;
            var offset = _parent.RectPivotOffset;
            ClickArea.RectPosition = halfPoint + offset - new Vector2(30, 30); // - halfPoint.Normalized() * 30;
        }

        private Vector2 GetTargetPoint()
        {
            var statePosition = _transition.GetParent<StateData>().Position;
            var targetStateName = _transition.Target;
            var stateMachine = _transition.GetParent<StateData>().GetParent<StateMachineData>();
            var targetState = stateMachine.States.FirstOrDefault(state => state.Name == targetStateName)
                              ?? throw new Exception($"State {targetStateName} not found");
            var target = targetState.Position - statePosition;
            return target.Normalized() * (target.Length() - LineShift);
        }
    }
}