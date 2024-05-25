using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class DpsCounterPlayer : PlayerHooks
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