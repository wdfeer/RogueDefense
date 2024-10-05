using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class SfxToggle : CheckButton
{
	public override void _Ready()
	{
		ButtonPressed = SaveManager.client.SoundOn;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), !SaveManager.client.SoundOn);

		Toggled += OnToggled;
	}

	void OnToggled(bool enabled)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), !enabled);
		SaveManager.client.SoundOn = enabled;
	}
}