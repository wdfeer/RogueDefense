using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Player.Hooks.Upgrades;

public class LowEnemyHpDamagePlayer : PlayerHooks
{
    public LowEnemyHpDamagePlayer(Core.Player player) : base(player)
    {
    }

    public float DmgIncrease => player.upgradeManager.SumAllUpgradeValues(UpgradeType.LowEnemyHpDamage);

    public bool IsConditionMet(Enemy.Enemy enemy)
    {
        return enemy.Hp <= enemy.maxHp * 0.5f;
    }
    public bool IsAffecting(Enemy.Enemy enemy)
    {
        return DmgIncrease > 0f && IsConditionMet(enemy);
    }

    public override void ModifyHitEnemyWithProj(Enemy.Enemy enemy, Projectile.Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult)
    {
        if (IsAffecting(enemy))
        {
            damagePreCrit *= 1f + DmgIncrease;
        }
    }
}