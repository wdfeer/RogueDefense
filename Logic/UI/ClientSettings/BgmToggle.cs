using Godot;
using RogueDefense;
using System;

public partial class BgmToggle : CheckButton
{
	public override void _Ready()
	{
		ButtonPressed = SaveData.Music;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !SaveData.Music);

		Toggled += OnToggled;
	}

	void OnToggled(bool enabled)
	{
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), !enabled);
		SaveData.Music = enabled;
	}
}
