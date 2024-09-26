using System;

namespace RogueDefense.Logic.Player.Hooks;

public class HpResetter : PlayerHooks
{
    public HpResetter(Core.Player player) : base(player)
    {
        if (!player.IsLocal) throw new ArgumentException("HpResetter must be on a local player");
    }

    public override void OnWaveEnd()
    {
        player.upgradeManager.UpdateMaxHp();
        DefenseObjective.instance.Hp = DefenseObjective.instance.maxHp;
    }
}