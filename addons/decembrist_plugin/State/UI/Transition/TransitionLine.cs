using System;
using System.Linq;
using Decembrist.Utils;
using Godot;
using StateMachineData = Decembrist.State.StateMachine;

namespace Decembrist.State.UI.Transition
{
    public class TransitionLine : Node2D
    {
        public const string NamePrefix = "TransitionTo";

        public const string LineNodeName = "TransitionLine";
        public const string ScriptLabelName = "ScriptLabel";
        public const string ClickAreaNodeName = "ClickArea";
        public const float LineShift = 70;
        public const float ArrowLength = 20f;

        [Signal]
        public delegate void Select();

        public readonly TransitionResource Resource;
        private readonly GraphNode _parent;
        private bool _selected = false;

        private Line2D Line => GetNode<Line2D>(LineNodeName);
        private Control ClickArea => GetNode<Control>(ClickAreaNodeName);
        private Label ScriptLabel => GetNode<Label>(ScriptLabelName);

        public TransitionLine(GraphNode parent, TransitionResource resource)
        {
            _parent = parent;
            Resource = resource;
            ShowBehindParent = true;
            Name = NamePrefix + resource.Target;
        }

        public override void _Draw()
        {
            AttachTransitionLine();
            AttachClickArea();
            if (Resource.Script != null) AttachScriptLabel();
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
                if (Resource.Script != null) ScriptLabel.Visible = false;
            }
        }

        public void SetSelected(bool selected) => _selected = selected;

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

        private void AttachClickArea()
        {
            var clickArea = new Panel
            {
                Name = ClickAreaNodeName,
                RectSize = new Vector2(60, 60),
                SelfModulate = Colors.Transparent,
            };
            AddChild(clickArea);
            clickArea.HintTooltip = Resource.Script;

            clickArea.OnMouseInput(
                _ => EmitSignal(nameof(Select)),
                @event => @event is {ButtonIndex: (int) ButtonList.Left, Pressed: true}
            );
        }

        private void AttachScriptLabel()
        {
            var scriptLabel = new Label
            {
                Text = Resource.Script?.Split("/").Last(),
                Name = ScriptLabelName,
            };
            AddChild(scriptLabel);
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
            SetUpScriptLabel(target);
        }

        private Vector2 GetTargetPoint()
        {
            var statePosition = _parent.Offset;
            var targetStateName = Resource.Target;
            var stateMachine = _parent.GetParent<GraphEdit>();
            var targetState =
                stateMachine.GetChildren<GraphNode>().FirstOrDefault(state => state.Name == targetStateName)
                ?? throw new Exception($"State {targetStateName} not found");
            var target = targetState.Offset - statePosition;
            return target.Normalized() * (target.Length() - LineShift);
        }

        private void SetUpScriptLabel(Vector2 target)
        {
            if (Resource.Script != null)
            {
                var halfPoint = target * new Vector2(0.85f, 0.85f);
                var offset = _parent.RectPivotOffset;
                ScriptLabel.Visible = true;
                var angleToTarget = (float) (180 / Math.PI) * Vector2.Zero.AngleToPoint(target);
                switch (angleToTarget)
                {
                    case >= 0 and < 41:
                        ScriptLabel.RectPosition = halfPoint + offset + new Vector2(40, -20);
                        ScriptLabel.RectRotation = angleToTarget - 40;
                        break;
                    case >= 41 and < 90:
                        ScriptLabel.RectPosition = halfPoint + offset + new Vector2(40, 0);
                        ScriptLabel.RectRotation = angleToTarget - 90;
                        break;
                    case >= 90 and < 140:
                        ScriptLabel.RectPosition = halfPoint + offset + new Vector2(20, 20);
                        ScriptLabel.RectRotation = angleToTarget - 90;
                        break;
                    case >= 140 and < 160:
                        ScriptLabel.RectPosition = halfPoint + offset + new Vector2(20, 0);
                        ScriptLabel.RectRotation = angleToTarget - 140;
                        break;
                    case >= 160 and <= 180:
                        ScriptLabel.RectPosition = halfPoint + offset + new Vector2(-30, -40);
                        ScriptLabel.RectRotation = angleToTarget + 125;
                        break;
                    case < 0 and > -70:
                        ScriptLabel.RectPosition = halfPoint + offset + new Vector2(30, 20);
                        ScriptLabel.RectRotation = angleToTarget + 40;
                        break;
                    case < -70 and > -140:
                        ScriptLabel.RectPosition = halfPoint + offset + new Vector2(30, -20);
                        ScriptLabel.RectRotation = angleToTarget + 90;
                        break;
                    case <= -140 and > -180:
                        ScriptLabel.RectPosition = halfPoint + offset + new Vector2(-10, -40);
                        ScriptLabel.RectRotation = angleToTarget + 115;
                        break;
                }
            }
        }
    }
}