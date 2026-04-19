using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Player.Hooks.Upgrades;

public class FourthShotMultishotPlayer : PlayerHooks
{
    int shots = 0;

    public FourthShotMultishotPlayer(Core.Player player) : base(player)
    {
    }

    public override void PreShoot(ShootManager shooter)
    {
        if (shots >= 4)
        {
            float mult = player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.FourthShotMultishot);
            shooter.multishot *= mult;

            shots = 0;
        }
        shots++;
    }
}