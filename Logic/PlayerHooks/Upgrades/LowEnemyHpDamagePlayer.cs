using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class LowEnemyHpDamagePlayer : PlayerHooks
    {
        public LowEnemyHpDamagePlayer(Player player) : base(player)
        {
        }

        public float buff = 0f;

        public bool IsConditionMet(Enemy enemy)
        {
            return enemy.Hp <= enemy.maxHp * 0.5f;
        }
        public bool IsAffecting(Enemy enemy)
        {
            return buff > 0f && IsConditionMet(enemy);
        }

        public override void ModifyHitEnemyWithBullet(Enemy enemy, Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult)
        {
            if (IsAffecting(enemy))
            {
                damagePreCrit *= 1f + buff;
            }
        }
    }
}