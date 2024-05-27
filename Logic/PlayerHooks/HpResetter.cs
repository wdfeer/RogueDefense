using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class HpResetter : PlayerHooks
{
    public HpResetter(Player player) : base(player) // Only created on the local player
    {
    }

    public override void OnWaveEnd()
    {
        player.upgradeManager.UpdateMaxHp();
        DefenseObjective.instance.Hp = DefenseObjective.instance.maxHp;
    }
}