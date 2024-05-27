using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class DamageVsArmorPlayer : PlayerHooks
{
    public DamageVsArmorPlayer(Player player) : base(player)
    {
    }

    public float mult = 1f;
    public override void ModifyHitEnemyWithProj(Enemy enemy, Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult)
    {
        if (enemy.armor > 0)
        {
            damagePreCrit *= mult;
        }
    }
}