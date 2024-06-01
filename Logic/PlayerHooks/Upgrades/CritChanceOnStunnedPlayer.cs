using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class CritChanceOnStunnedPlayer : PlayerHooks
{
    public CritChanceOnStunnedPlayer(Player player) : base(player)
    {
        Game.instance.GetNode<BuffText>("BuffText").textGetters.Add(GetBuffText);
    }

    public float crit = 0;
    public const int DURATION = 5;
    public float time = 0;
    public void Activate()
    {
        if (crit <= 0)
            return;

        time = DURATION;
    }
    public override void PostUpgradeUpdate(float delta)
    {
        if (crit <= 0 || time <= 0)
            return;

        player.upgradeManager.critChance += crit;
        time -= delta;
    }
    string GetBuffText()
    {
        if (crit <= 0 || time <= 0)
            return "";
        return $"+{MathHelper.ToPercentAndRound(crit)}% Crit Chance";
    }
}