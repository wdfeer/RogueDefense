using System.Linq;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic.PlayerHooks.Upgrades;

public class MaxHpPerKillPlayer : PlayerHooks
{
    public float increase = 0f; // Used in UpgradeManager

    public MaxHpPerKillPlayer(Player player) : base(player)
    {
    }

    public static float GetTotalIncrease()
        => PlayerManager.players
            .Select(pair => GetHooks<MaxHpPerKillPlayer>(pair.Value).increase)
            .Aggregate(0f, (a, b) => a + b);
    public override void OnKill(Enemy enemy)
    {
        increase += player.upgradeManager.SumAllUpgradeValues(UpgradeType.MaxHpPerKill);
    }
}