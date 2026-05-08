using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.UI.InGame;

public partial class PauseMenu : Control
{
    [Export] public Button resumeButton;
    [Export] public Button exitButton;

    ConfirmationDialog ConfirmationPopup => exitButton.GetNode<ConfirmationDialog>("ConfirmPopup");

    public override void _Ready()
    {
        ConfirmationPopup.GetOkButton().Pressed += () => { Game.instance.GoToMainMenu(); };
        exitButton.Pressed += () => { ConfirmationPopup.Popup(); };
        resumeButton.Pressed += () => { Visible = false; };
        VisibilityChanged += () =>
        {
            if (NetworkManager.Singleplayer)
            {
                GetTree().Paused = Visible;
            }
        };
    }
}