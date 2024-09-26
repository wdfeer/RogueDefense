using Godot;
using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class BgmToggle : CheckButton
{
	public override void _Ready()
	{
		ButtonPressed = UserData.Music;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !UserData.Music);

		Toggled += OnToggled;
	}

	void OnToggled(bool enabled)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !enabled);
		UserData.Music = enabled;
	}
}