using Godot;
using System;
using RogueDefense;
public class HpBarButton : CheckBox
{
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => Pressed = SaveData.showHpBar);
    }
    public override void _Toggled(bool buttonPressed)
    {
        SaveData.showHpBar = buttonPressed;
    }
}
