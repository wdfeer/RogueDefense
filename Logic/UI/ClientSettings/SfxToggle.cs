using Godot;

namespace RogueDefense.Logic.UI.ClientSettings;

public partial class SfxToggle : CheckButton
{
	public override void _Ready()
	{
		ButtonPressed = SaveData.Sound;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), !SaveData.Sound);

		Toggled += OnToggled;
	}

	void OnToggled(bool enabled)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), !enabled);
		SaveData.Sound = enabled;
	}
}