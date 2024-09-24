using System.Collections.Generic;
using Godot;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic;

public partial class GameInitializer : Node
{
	public override void _Ready()
	{
		Player.my = null;
		Player.players = new Dictionary<int, Player>();

		SaveData.Load();

		UpgradeType.Initialize();
	}
}