using Godot;
using RogueDefense;
using System;

public partial class AvgDPSButton : CheckBox
{
	public override void _Ready()
	{
		ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => ButtonPressed = SaveData.showAvgDPS);
	}
	public override void _Toggled(bool buttonPressed)
	{
		SaveData.showAvgDPS = buttonPressed;
	}
}
