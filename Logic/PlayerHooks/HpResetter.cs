using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class HpResetter : PlayerHooks
    {
        public HpResetter(Player player) : base(player)
        {
        }

        public override void OnKill()
        {
            player.upgradeManager.UpdateMaxHp();
            DefenseObjective.instance.Hp = DefenseObjective.instance.maxHp;
        }
    }
}