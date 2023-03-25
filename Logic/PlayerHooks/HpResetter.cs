using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class HpResetter : PlayerHooks
    {
        public override void OnKill()
        {
            Player.upgradeManager.UpdateMaxHp();
            DefenseObjective.instance.Hp = DefenseObjective.instance.maxHp;
        }
    }
}