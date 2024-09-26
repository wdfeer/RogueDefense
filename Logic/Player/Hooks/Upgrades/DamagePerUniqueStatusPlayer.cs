using System.Linq;
using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Player.Hooks.Upgrades;

public class DamagePerUniqueStatusPlayer : PlayerHooks
{
    public float IncreasePerStatus => player.upgradeManager.SumAllUpgradeValues(UpgradeType.DamagePerUniqueStatus);

    public DamagePerUniqueStatusPlayer(Core.Player player) : base(player)
    {
    }

    public override void ModifyHitEnemyWithProj(Enemy.Enemy enemy, Projectile.Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult)
    {
        int statuses = enemy.statuses.Count(x => x.Active);
        if (statuses <= 0)
            return;
        damagePreCrit *= 1f + IncreasePerStatus * statuses;
    }
}