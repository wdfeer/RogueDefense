using System.Collections.Generic;
using Godot;
using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic;

public partial class GameInitializer : Node
{
	public override void _Ready()
	{
		PlayerManager.my = null;
		PlayerManager.players = new Dictionary<int, Player.Core.Player>();

		SaveData.Load();

		UpgradeType.Initialize();
	}
}