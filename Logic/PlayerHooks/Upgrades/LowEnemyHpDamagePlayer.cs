using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class LowEnemyHpDamagePlayer : PlayerHooks
    {
        public override void ModifyHitWithBullet(Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult)
        {
            if (Game.instance.enemy.Hp <= Game.instance.enemy.maxHp * 0.5f)
            {
                float mult = Player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.LowEnemyHpDamage);
                damagePreCrit *= mult;
            }
        }
    }
}