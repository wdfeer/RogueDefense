using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.InGame;

public partial class FpsLabel : Label
{
	public override void _Ready()
	{
		Visible = SaveManager.client.ShowFps;
	}
	public override void _Process(double delta)
	{
		Visible = SaveManager.client.ShowFps;
		if (!Visible)
			return;

		Text = Performance.GetMonitor(Performance.Monitor.TimeFps).ToString();
	}
}