using Godot;
using System;

public class Sliders : VBoxContainer
{
    public static SettingsSlider dmgMult;
    public override void _Ready()
    {
        dmgMult = GetNode<SettingsSlider>("DmgMult");

        ToSignal(GetTree().CreateTimer(0.01f), "timeout").OnCompleted(() =>
        {
            GameSettings.UpdateFromSliders();
        });
    }
}
