using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class BgmToggle : CheckButton
{
	public override void _Ready()
	{
		ButtonPressed = UserData.clientSettings.MusicOn;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !UserData.clientSettings.MusicOn);

		Toggled += OnToggled;
	}

	void OnToggled(bool enabled)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !enabled);
		UserData.clientSettings.MusicOn = enabled;
	}
}