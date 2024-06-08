using Godot;
using RogueDefense;

public partial class ShowFpsButton : CheckBox
{
	public override void _Ready()
	{
		ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => ButtonPressed = SaveData.ShowAvgDPS);

		Toggled += OnToggled;
	}

	void OnToggled(bool toggled)
	{
		SaveData.ShowFPS = toggled;
	}
}
