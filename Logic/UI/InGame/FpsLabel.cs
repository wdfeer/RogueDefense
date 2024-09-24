using Godot;

namespace RogueDefense.Logic.UI.InGame;

public partial class FpsLabel : Label
{
	public override void _Ready()
	{
		Visible = SaveData.ShowFPS;
	}
	public override void _Process(double delta)
	{
		Visible = SaveData.ShowFPS;
		if (!Visible)
			return;

		Text = Performance.GetMonitor(Performance.Monitor.TimeFps).ToString();
	}
}