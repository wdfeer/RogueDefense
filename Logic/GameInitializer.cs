using System.Collections.Generic;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Save;

namespace RogueDefense.Logic;

public partial class GameInitializer : Node
{
	public override void _Ready()
	{
		PlayerManager.my = null;
		PlayerManager.players = new Dictionary<int, Player.Core.Player>();

		SaveManager.Load();

		UpgradeType.Initialize();
	}
}