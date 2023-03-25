using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class DpsCounterPlayer : PlayerHooks
    {
        public DpsCounterPlayer(Player player) : base(player)
        {
        }

        public override void OnAnyHit(float afterEffectsDmg)
        {
            DpsLabel.instance.hits.Add((afterEffectsDmg, 0f));
        }
    }
}