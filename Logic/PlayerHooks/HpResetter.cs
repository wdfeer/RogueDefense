using System;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic.PlayerHooks;

public class HpResetter : PlayerHooks
{
    public HpResetter(Player player) : base(player)
    {
        if (!player.IsLocal) throw new ArgumentException("HpResetter must be on a local player");
    }

    public override void OnWaveEnd()
    {
        player.upgradeManager.UpdateMaxHp();
        DefenseObjective.instance.Hp = DefenseObjective.instance.maxHp;
    }
}