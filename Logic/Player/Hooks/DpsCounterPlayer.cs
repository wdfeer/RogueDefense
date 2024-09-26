namespace RogueDefense.Logic.Player.Hooks;

public class DpsCounterPlayer : PlayerHooks
{
    public DpsCounterPlayer(Core.Player player) : base(player) { }

    public override void OnAnyHit(float afterEffectsDmg)
    {
        UI.InGame.DpsLabel.instance.hits.Add((afterEffectsDmg, 0f));
    }
}