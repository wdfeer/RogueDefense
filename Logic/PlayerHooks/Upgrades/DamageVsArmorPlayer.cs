using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public class DamageVsArmorPlayer : PlayerHooks
{
    public DamageVsArmorPlayer(Player player) : base(player)
    {
    }

    public float Mult => 1f + player.upgradeManager.SumAllUpgradeValues(UpgradeType.TotalDamageVsArmor);
    public override void ModifyHitEnemyWithProj(Enemy enemy, Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult)
    {
        if (enemy.armor > 0)
        {
            damagePreCrit *= Mult;
        }
    }
}