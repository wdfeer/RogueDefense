using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public class LowEnemyHpDamagePlayer : PlayerHooks
    {
        public bool ConditionMet => Game.instance.enemy.Hp <= Game.instance.enemy.maxHp * 0.5f;
        public float buff = 0f;

        public LowEnemyHpDamagePlayer(Player player) : base(player)
        {
        }

        public bool Affecting => buff > 0f && ConditionMet;
        public override void ModifyHitWithBullet(Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult)
        {
            if (Affecting)
            {
                damagePreCrit *= 1f + buff;
            }
        }
    }
}