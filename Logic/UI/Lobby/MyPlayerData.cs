using System.Linq;
using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.Lobby;

public partial class MyPlayerData : PlayerData
{
	public override void _Ready()
	{
		SetPlayerName(SaveManager.user.name);
		SetAugmentPoints(SaveManager.user.augmentAllotment.Sum());
	}
}