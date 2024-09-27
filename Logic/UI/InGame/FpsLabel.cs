using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.InGame;

public partial class FpsLabel : Label
{
	public override void _Ready()
	{
		Visible = UserData.ShowFPS;
	}
	public override void _Process(double delta)
	{
		Visible = UserData.ShowFPS;
		if (!Visible)
			return;

		Text = Performance.GetMonitor(Performance.Monitor.TimeFps).ToString();
	}
}