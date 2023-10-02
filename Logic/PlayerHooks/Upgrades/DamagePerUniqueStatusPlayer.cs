using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class DamagePerUniqueStatusPlayer : PlayerHooks
    {
        public float damageIncreasePerUniqueStatus = 0f;

        public DamagePerUniqueStatusPlayer(Player player) : base(player)
        {
        }

        public override void ModifyHitWithBullet(Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult)
        {
            int statuses = Enemy.instance.statuses.Count(x => x.Active);
            if (statuses <= 0)
                return;
            damagePreCrit *= 1f + damageIncreasePerUniqueStatus * statuses;
        }
    }
}