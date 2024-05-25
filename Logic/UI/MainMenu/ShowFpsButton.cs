using Godot;
using RogueDefense;
using System;

public partial class ShowFpsButton : CheckBox
{
	public override void _Ready()
	{
		Toggled += OnToggled;
	}

	void OnToggled(bool toggled)
	{
		SaveData.showFPS = toggled;
	}
}
