using Godot;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class AvgDPSButton : CheckBox
{
	public override void _Ready()
	{
		ToSignal(GetTree().CreateTimer(0.001f), "timeout").OnCompleted(() => ButtonPressed = SaveData.ShowAvgDPS);
	}
	public override void _Toggled(bool buttonPressed)
	{
		SaveData.ShowAvgDPS = buttonPressed;
	}
}