using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public class DamagePerUniqueStatusPlayer : PlayerHooks
{
    public float IncreasePerStatus => player.upgradeManager.SumAllUpgradeValues(UpgradeType.DamagePerUniqueStatus);

    public DamagePerUniqueStatusPlayer(Player player) : base(player)
    {
    }

    public override void ModifyHitEnemyWithProj(Enemy enemy, Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult)
    {
        int statuses = enemy.statuses.Count(x => x.Active);
        if (statuses <= 0)
            return;
        damagePreCrit *= 1f + IncreasePerStatus * statuses;
    }
}