using Godot;
using RogueDefense;
using System;

public class ExitButton : Button
{
    ConfirmationDialog ConfirmationPopup => (ConfirmationDialog)GetNode("ConfirmPopup");
    public override void _Ready()
    {
        ConfirmationPopup.GetOk().Connect("pressed", this, "ExitConfirmed");
    }
    public void ExitConfirmed()
    {
        Player.localInstance.hpManager.Death();
    }
    public override void _Pressed()
    {
        ConfirmationPopup.Show();
    }
}
