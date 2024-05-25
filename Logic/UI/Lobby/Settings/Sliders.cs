using Godot;

public partial class Sliders : VBoxContainer
{
    public static SettingsSlider dmgMult;
    public static SettingsSlider fireRateMult;
    public static SettingsSlider healthDrain;

    public override void _Ready()
    {
        dmgMult = GetNode<SettingsSlider>("DmgMult");
        fireRateMult = GetNode<SettingsSlider>("FireRateMult");
        healthDrain = GetNode<SettingsSlider>("HpDrain");

        ToSignal(GetTree().CreateTimer(0.01f), "timeout").OnCompleted(() =>
        {
            GameSettings.UpdateFromSliders();
        });
    }
}
