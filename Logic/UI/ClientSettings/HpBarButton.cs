using Godot;
using RogueDefense;
public partial class HpBarButton : CheckBox
{
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => ButtonPressed = SaveData.ShowHpBar);
    }
    public override void _Toggled(bool buttonPressed)
    {
        SaveData.ShowHpBar = buttonPressed;
    }
}
