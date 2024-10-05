using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class ShowFpsButton : CheckBox
{
	public override void _Ready()
	{
		ToSignal(GetTree().CreateTimer(0.001f), "timeout")
			.OnCompleted(() => ButtonPressed = UserData.clientSettings.ShowFps);

		Toggled += OnToggled;
	}

	void OnToggled(bool toggled)
	{
		UserData.clientSettings.ShowFps = toggled;
	}
}