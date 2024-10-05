using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class BgmToggle : CheckButton
{
	public override void _Ready()
	{
		ButtonPressed = SaveManager.client.MusicOn;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !SaveManager.client.MusicOn);

		Toggled += OnToggled;
	}

	void OnToggled(bool enabled)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !enabled);
		SaveManager.client.MusicOn = enabled;
	}
}