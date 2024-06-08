using Godot;
using RogueDefense;
using System;

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
