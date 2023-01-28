using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class FirstHitPlayer : PlayerHooks
    {
        public override void ModifyHitWithBullet(Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult)
        {
            if (Game.instance.enemy.Hp >= Game.instance.enemy.maxHp)
            {
                float mult = Player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.FirstHitCritDamage);
                critMult *= mult;
            }
        }
    }
}