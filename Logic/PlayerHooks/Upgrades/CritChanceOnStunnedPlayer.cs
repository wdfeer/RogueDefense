using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class CritChanceOnStunnedPlayer : PlayerHooks
{
    public CritChanceOnStunnedPlayer(Player player) : base(player)
    {
        Game.instance.GetNode<BuffText>("BuffText").textGetters.Add(GetBuffText);
    }

    public float Crit => player.upgradeManager.SumAllUpgradeValues(UpgradeType.CritChanceOnStunned);
    public const int DURATION = 5;
    public float time = 0;
    public void Activate()
    {
        if (Crit <= 0)
            return;

        time = DURATION;
    }
    public override void PostUpgradeUpdate(float delta)
    {
        if (Crit <= 0 || time <= 0)
            return;

        player.upgradeManager.critChance += Crit;
        time -= delta;
    }
    string GetBuffText()
    {
        if (Crit <= 0 || time <= 0)
            return "";
        return $"+{MathHelper.ToPercentAndRound(Crit)}% Crit Chance";
    }
}