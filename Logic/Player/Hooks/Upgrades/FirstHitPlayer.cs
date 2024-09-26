using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Player.Hooks.Upgrades;

public class FirstHitPlayer : PlayerHooks
{
    public FirstHitPlayer(Core.Player player) : base(player)
    {
    }

    public override void ModifyHitEnemyWithProj(Enemy.Enemy enemy, Projectile.Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult)
    {
        if (enemy.Hp >= enemy.maxHp)
        {
            float mult = player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.FirstHitCritDamage);
            critMult *= mult;
        }
    }
}