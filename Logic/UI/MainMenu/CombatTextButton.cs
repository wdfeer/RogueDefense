using Godot;
using System;
using RogueDefense;
public class CombatTextButton : CheckBox
{
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => Pressed = SaveData.showCombatText);
    }
    public override void _Toggled(bool buttonPressed)
    {
        SaveData.showCombatText = buttonPressed;
    }
}
