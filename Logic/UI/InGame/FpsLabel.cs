using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.InGame;

public partial class FpsLabel : Label
{
	public override void _Ready()
	{
		Visible = UserData.clientSettings.ShowFps;
	}
	public override void _Process(double delta)
	{
		Visible = UserData.clientSettings.ShowFps;
		if (!Visible)
			return;

		Text = Performance.GetMonitor(Performance.Monitor.TimeFps).ToString();
	}
}