using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class FirstHitPlayer : PlayerHooks
{
    public FirstHitPlayer(Player player) : base(player)
    {
    }

    public override void ModifyHitEnemyWithProj(Enemy enemy, Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult)
    {
        if (enemy.Hp >= enemy.maxHp)
        {
            float mult = player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.FirstHitCritDamage);
            critMult *= mult;
        }
    }
}