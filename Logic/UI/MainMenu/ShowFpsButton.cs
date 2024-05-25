using Godot;
using RogueDefense;
using System;

public partial class ShowFpsButton : CheckBox
{
	public override void _Ready()
	{
		ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => ButtonPressed = SaveData.showAvgDPS);

		Toggled += OnToggled;
	}

	void OnToggled(bool toggled)
	{
		SaveData.showFPS = toggled;
	}
}
