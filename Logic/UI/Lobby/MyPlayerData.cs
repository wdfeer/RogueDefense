using Godot;
using RogueDefense;
using System;
using System.Linq;

public partial class MyPlayerData : PlayerData
{
	public override void _Ready()
	{
		SetPlayerName(SaveData.name);
		SetAugmentPoints(SaveData.augmentAllotment.Sum());
	}
}
