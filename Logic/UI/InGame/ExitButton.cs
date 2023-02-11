using Godot;
using RogueDefense;
using System;

public class ExitButton : Button
{
    ConfirmationDialog ConfirmationPopup => (ConfirmationDialog)GetNode("ConfirmPopup");
    public override void _Ready()
    {
        ConfirmationPopup.GetOk().Connect("pressed", this, "ExitConfirmed");
        popupBounds = ConfirmationPopup.GetRect();
    }
    public void ExitConfirmed()
    {
        Player.localInstance.hpManager.Death();
    }
    Rect2 popupBounds;
    public override void _Pressed()
    {
        ConfirmationPopup.Popup_(popupBounds);
    }
}
