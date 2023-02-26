using Godot;
using System;

public class Sliders : VBoxContainer
{
    public static SettingsSlider dmgMult;
    public static SettingsSlider fireRateMult;

    public override void _Ready()
    {
        dmgMult = GetNode<SettingsSlider>("DmgMult");
        fireRateMult = GetNode<SettingsSlider>("FireRateMult");

        ToSignal(GetTree().CreateTimer(0.01f), "timeout").OnCompleted(() =>
        {
            GameSettings.UpdateFromSliders();
        });
    }
}
