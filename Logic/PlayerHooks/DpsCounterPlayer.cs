using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic.PlayerHooks;

public class DpsCounterPlayer : PlayerHooks
{
    public DpsCounterPlayer(Player player) : base(player)
    {
    }

    public override void OnAnyHit(float afterEffectsDmg)
    {
        UI.InGame.DpsLabel.instance.hits.Add((afterEffectsDmg, 0f));
    }
}