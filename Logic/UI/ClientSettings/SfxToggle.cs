using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class SfxToggle : CheckButton
{
	public override void _Ready()
	{
		ButtonPressed = UserData.clientSettings.SoundOn;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), !UserData.clientSettings.SoundOn);

		Toggled += OnToggled;
	}

	void OnToggled(bool enabled)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), !enabled);
		UserData.clientSettings.SoundOn = enabled;
	}
}