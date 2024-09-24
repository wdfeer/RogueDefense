using System.Linq;

namespace RogueDefense.Logic.UI.Lobby;

public partial class MyPlayerData : PlayerData
{
	public override void _Ready()
	{
		SetPlayerName(SaveData.name);
		SetAugmentPoints(SaveData.augmentAllotment.Sum());
	}
}