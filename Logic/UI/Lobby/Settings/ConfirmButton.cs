using Godot;
using System;

public class ConfirmButton : Button
{
    public override void _Pressed()
    {
        SettingsButton.panel.Visible = false;

        GameSettings.UpdateFromSliders();
        GameSettings.SyncSettings();
    }
}
