using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class SfxToggle : CheckButton
{
	public override void _Ready()
	{
		ButtonPressed = UserData.Sound;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), !UserData.Sound);

		Toggled += OnToggled;
	}

	void OnToggled(bool enabled)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), !enabled);
		UserData.Sound = enabled;
	}
}