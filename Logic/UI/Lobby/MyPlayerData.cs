using Godot;
using RogueDefense;
using System;

public partial class MyPlayerData : PlayerData
{
	public override void _Ready()
	{
		SetPlayerName(SaveData.name);
	}
}
