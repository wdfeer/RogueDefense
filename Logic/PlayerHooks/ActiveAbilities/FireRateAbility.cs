using Godot;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class FireRateAbility : ActiveAbility
{
    public override void Activate()
    {
        buffLeft = Duration * 5f;
    }
    public float buffLeft = 0;

    public FireRateAbility(Player player, Button button) : base(player, button)
    {
    }

    public override void PostUpgradeUpdate(float delta)
    {
        if (buffLeft > 0)
        {
            buffLeft -= delta;
            player.shootManager.shootInterval /= 1 + 0.75f * Strength;
        }
    }
    public override float BaseCooldown => 22f;
    protected override string GetAbilityText()
        => $@"+{(int)(75f * Strength)}% Total Fire Rate and
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown:0.00} s";
}