using Godot;
using RogueDefense;
using System;

public partial class ExitButton : TextureButton
{
    ConfirmationDialog ConfirmationPopup => (ConfirmationDialog)GetNode("ConfirmPopup");
    public override void _Ready()
    {
        ConfirmationPopup.GetOkButton().Connect("pressed", new Callable(this, "ExitConfirmed"));
        popupBounds = ConfirmationPopup.GetVisibleRect();
    }
    public void ExitConfirmed()
    {
        DefenseObjective.instance.Death();
    }
    Rect2 popupBounds;
    public override void _Pressed()
    {
        ConfirmationPopup.Popup((Rect2I)popupBounds);
    }
}
