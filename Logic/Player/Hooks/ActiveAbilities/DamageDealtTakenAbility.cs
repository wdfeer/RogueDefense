using Godot;
using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Player.Hooks.ActiveAbilities;

public class DamageDealtTakenAbility : ActiveAbility
{
    public DamageDealtTakenAbility(Core.Player player, Button button) : base(player, button)
    {
    }

    public override void Activate()
    {
        buffLeft = Duration * 10f;
    }
    public float buffLeft = 0;
    public override void PostUpgradeUpdate(float delta)
    {
        if (buffLeft > 0 && player.IsLocal)
        {
            buffLeft -= delta;
            DefenseObjective.instance.damageMult *= 1f + DamageTaken;
        }
    }
    public override void PreShoot(ShootManager shooter)
    {
        if (buffLeft > 0)
        {
            shooter.damage *= 1f + 2f * Strength;
            shooter.colored = true;
        }
    }
    public float DamageTaken => 0.5f * Strength;
    public override float BaseCooldown => 60f;
    protected override string GetAbilityText()
        => $@"+{(int)(200f * Strength)}% Total Damage but
+{MathHelper.ToPercentAndRound(DamageTaken)}% Damage Taken
Duration: {(10f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
}