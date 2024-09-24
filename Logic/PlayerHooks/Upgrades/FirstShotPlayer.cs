using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public class FirstShotPlayer : PlayerHooks
{
    public float DamageMult => 1f + player.upgradeManager.SumAllUpgradeValues(UpgradeType.FirstShotTotalDamage);

    public FirstShotPlayer(Player player) : base(player)
    {
    }

    public override void PreShoot(ShootManager shooter)
    {
        if (DamageMult <= 1f || shooter.shootCount > 0)
        {
            return;
        }

        shooter.damage *= DamageMult;
        shooter.colored = true;
    }
}