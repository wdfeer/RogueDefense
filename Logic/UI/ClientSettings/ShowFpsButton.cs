using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class ShowFpsButton : CheckBox
{
	public override void _Ready()
	{
		ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => ButtonPressed = UserData.ShowAvgDPS);

		Toggled += OnToggled;
	}

	void OnToggled(bool toggled)
	{
		UserData.ShowFPS = toggled;
	}
}