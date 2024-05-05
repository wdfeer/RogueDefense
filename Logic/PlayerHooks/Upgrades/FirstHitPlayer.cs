using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class FirstHitPlayer : PlayerHooks
    {
        public FirstHitPlayer(Player player) : base(player)
        {
        }

        public override void ModifyHitEnemyWithBullet(Enemy enemy, Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult)
        {
            if (enemy.Hp >= enemy.maxHp)
            {
                float mult = player.upgradeManager.GetTotalUpgradeMultiplier(UpgradeType.FirstHitCritDamage);
                critMult *= mult;
            }
        }
    }
}