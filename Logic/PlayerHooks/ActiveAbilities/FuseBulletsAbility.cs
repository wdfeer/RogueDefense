using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using RogueDefense.Logic.PlayerProjectile;

namespace RogueDefense;

public partial class FuseBulletsAbility : ActiveAbility
{
    public FuseBulletsAbility(Player player, Button button) : base(player, button)
    {
        if (button != null)
        {
            button.Icon = (Texture2D)GD.Load("res://Assets/Images/Icons/game-icons.net/concentration-orb.svg");
        }
    }

    public override bool CanBeActivated()
        => player.shootManager.projectileManager.proj.Any(x => x is not FusedBullet);
    public override void Activate()
    {
        ShootManager shooter = player.shootManager;

        float hitMult = shooter.projectileManager.proj.Aggregate(0f, (a, b) =>
            a + (b is FusedBullet ? 0 : b.hitMult));
        shooter.ClearBullets(x => x is FusedBullet);

        Vector2 pos = player.controlledTurret.bulletSpawnpoint.GlobalPosition;
        FusedBullet bullet = new(shooter.projectileManager.textures)
        {
            owner = player,
            position = pos,
            velocity = pos.DirectionTo(player.target.GlobalPosition) * ShootManager.BASE_SHOOT_SPEED,
            damage = shooter.damage,
        };
        bullet.SetHitMultiplier(hitMult * (1f + PowerMultBonus));

        shooter.projectileManager.proj.Add(bullet);
    }
    public override bool Shared => false;
    public float PowerMultBonus => 1f * Strength;
    public override float BaseCooldown => 10f / Mathf.Sqrt(Duration);
    protected override string GetAbilityText()
        => $@"Fuse all your bullets into one
with +{MathHelper.ToPercentAndRound(PowerMultBonus)}% Power
Cooldown: {Cooldown:0.00} s";
}