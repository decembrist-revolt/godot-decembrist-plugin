#nullable enable
using System;
using Decembrist.Utils;
using Godot;

namespace Decembrist.State.UI.State
{
    public class StatePanelController
    {
        private PopupPanel _statePanel;

        private Label _panelLabel;
        private Button _actionButton;
        private LineEdit _nameEdit;
        private Button _cancelButton;

        public StatePanelController(PopupPanel statePanel)
        {
            _statePanel = statePanel;

            _actionButton = statePanel.FindNode<Button>("ActionButton");
            _panelLabel = statePanel.FindNode<Label>("Label");
            _nameEdit = statePanel.FindNode<LineEdit>("StateNameEdit");
            _cancelButton = statePanel.FindNode<Button>("CancelButton");

            _cancelButton.OnButtonPress(_statePanel.Hide);
        }

        public void PopupStateEdit(
            string panelLabel,
            string name,
            string actionName,
            Action<string?> action,
            Func<string?, bool> validateName
        )
        {
            _panelLabel.Text = panelLabel;
            _actionButton.Disabled = true;
            _nameEdit.Text = name;
            _nameEdit.Editable = true;
            _actionButton.Text = actionName;
            var cancelActionSubscription = _actionButton.OnButtonPress(() => ActionPress(action));
            var cancelEditSubscription = _nameEdit.OnTextChanged(text => _actionButton.Disabled = !validateName(text));
            _statePanel.PopupCentered(new Vector2(450, 350));
            _statePanel.OnPopupHide(() =>
            {
                cancelEditSubscription();
                cancelActionSubscription();
            });
        }

        private void ActionPress(Action<string?> action)
        {
            action(_nameEdit.Text);
            _statePanel.Hide();
        }
    }
}