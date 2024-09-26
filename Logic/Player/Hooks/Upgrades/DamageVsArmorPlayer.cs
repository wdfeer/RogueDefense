using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Player.Hooks.Upgrades;

public class DamageVsArmorPlayer : PlayerHooks
{
    public DamageVsArmorPlayer(Core.Player player) : base(player)
    {
    }

    public float Mult => 1f + player.upgradeManager.SumAllUpgradeValues(UpgradeType.TotalDamageVsArmor);
    public override void ModifyHitEnemyWithProj(Enemy.Enemy enemy, Projectile.Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult)
    {
        if (enemy.armor > 0)
        {
            damagePreCrit *= Mult;
        }
    }
}