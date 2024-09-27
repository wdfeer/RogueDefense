namespace RogueDefense.Logic.UI.InGame;

public partial class ExitButton : Button
{
    ConfirmationDialog ConfirmationPopup => (ConfirmationDialog)GetNode("ConfirmPopup");
    public override void _Ready()
    {
        ConfirmationPopup.GetOkButton().Connect("pressed", new Callable(this, "ExitConfirmed"));
        popupBounds = ConfirmationPopup.GetVisibleRect();
    }
    public void ExitConfirmed()
    {
        Game.instance.GoToMainMenu();
    }
    Rect2 popupBounds;
    public override void _Pressed()
    {
        ConfirmationPopup.Popup((Rect2I)popupBounds);
    }
}