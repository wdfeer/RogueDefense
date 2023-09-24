using Godot;
using System;

public partial class ConfirmButton : Button
{
    public override void _Pressed()
    {
        SettingsButton.panel.Visible = false;

        GameSettings.UpdateFromSliders();
        GameSettings.SendSettings();
    }
}
