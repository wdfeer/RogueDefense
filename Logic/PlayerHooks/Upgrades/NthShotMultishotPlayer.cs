using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public class NthShotMultishotPlayer : PlayerHooks
{
    int shots = 0;

    public NthShotMultishotPlayer(Player player) : base(player)
    {
    }

    public override void PreShoot(ShootManager shooter)
    {
        if (shots >= 4)
        {
            float mult = player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.NthShotMultishot);
            shooter.multishot *= mult;

            shots = 0;
        }
        shots++;
    }
}