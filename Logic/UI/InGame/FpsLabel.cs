using Godot;
using RogueDefense;

public partial class FpsLabel : Label
{
	public override void _Ready()
	{
		Visible = SaveData.showFPS;
	}
	public override void _Process(double delta)
	{
		Visible = SaveData.showFPS;
		if (!Visible)
			return;

		Text = Performance.GetMonitor(Performance.Monitor.TimeFps).ToString();
	}
}
