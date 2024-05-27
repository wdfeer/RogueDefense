using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class FirstShotPlayer : PlayerHooks
{
    public float damageMult = 1f;

    public FirstShotPlayer(Player player) : base(player)
    {
    }

    public override void PreShoot(ShootManager shooter)
    {
        if (damageMult <= 1f || shooter.shootCount > 0)
        {
            return;
        }

        shooter.damage *= damageMult;
        shooter.colored = true;
    }
}