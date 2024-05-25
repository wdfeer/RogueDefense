using Godot;
using RogueDefense;
using System;

public partial class FpsLabel : Label
{
	public override void _Process(double delta)
	{
		Visible = SaveData.showFPS;
		if (!Visible)
			return;

		Text = Performance.GetMonitor(Performance.Monitor.TimeFps).ToString();
	}
}
