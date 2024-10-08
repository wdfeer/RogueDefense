using System.Linq;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Projectile;

namespace RogueDefense.Logic.Player.Hooks.ActiveAbilities;

public class FuseBulletsAbility : ActiveAbility
{
    public FuseBulletsAbility(Core.Player player, Button button) : base(player, button)
    {
        if (button != null)
        {
            button.Icon = (Texture2D)GD.Load("res://Assets/Images/Icons/game-icons.net/concentration-orb.svg");
        }
    }

    public override bool CanBeActivated()
        => Enumerable.Any<Projectile.Projectile>(player.shootManager.projectileManager.proj, x => x is not FusedBullet);
    public override void Activate()
    {
        ShootManager shooter = player.shootManager;

        int hitMult = shooter.projectileManager.proj.Aggregate(0, (a, b) =>
            a + (b is FusedBullet ? 0 : b.hitMult));
        shooter.ClearBullets(x => x is FusedBullet);

        Vector2 pos = player.controlledTurret.bulletSpawnpoint.GlobalPosition;
        Vector2 velocity = pos.DirectionTo(player.target.GlobalPosition) * ShootManager.BASE_SHOOT_SPEED;
        FusedBullet bullet = new(shooter.projectileManager.textures)
        {
            owner = player,
            position = pos,
            velocity = velocity,
            damage = shooter.damage,
        };
        bullet.SetHitMultiplier(hitMult * (1f + PowerMultBonus));

        shooter.projectileManager.proj.Add(bullet);

        player.controlledTurret.Velocity += -velocity * 70 * Mathf.Sqrt(hitMult);
    }
    public override bool Shared => false;
    public float PowerMultBonus => 1f * Strength;
    public override float BaseCooldown => 15f / Mathf.Sqrt(Duration);
    protected override string GetAbilityText()
        => $@"Fuse all your bullets into one
with +{MathHelper.ToPercentAndRound(PowerMultBonus)}% Power
Cooldown: {Cooldown:0.00} s";
}