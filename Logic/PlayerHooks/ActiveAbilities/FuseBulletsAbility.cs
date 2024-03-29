using System.Linq;
using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class FuseBulletsAbility : ActiveAbility
    {
        public FuseBulletsAbility(Player player, Button button) : base(player, button)
        {
            if (button != null)
            {
                button.Icon = (Texture2D)GD.Load("res://Assets/game-icons.net/concentration-orb.svg");
            }
        }

        public override bool CanBeActivated()
            => player.shootManager.bullets.Any(x => GodotObject.IsInstanceValid(x) && !x.fused);
        public override void Activate()
        {
            ShootManager shooter = player.shootManager;

            float hitMult = shooter.bullets.Aggregate(0f, (a, b) =>
                a + ((GodotObject.IsInstanceValid(b) && !b.fused) ? b.hitMult : 0));
            shooter.ClearBullets(x => !x.fused);
            Bullet bullet = shooter.Shoot(player.shootManager.bulletSpawns[0].GlobalPosition, 3f);
            bullet.SetHitMultiplier(hitMult * (1f + PowerMultBonus));
            bullet.damage = shooter.damage;
            bullet.Scale *= 2f;
            bullet.StartParticleEffect();
            bullet.fused = true;
        }
        public override bool Shared => false;
        public float PowerMultBonus => 1.5f * Strength;
        public override float BaseCooldown => 10f / Mathf.Sqrt(Duration);
        protected override string GetAbilityText()
            => $@"Fuse all bullets on screen into one
with +{MathHelper.ToPercentAndRound(PowerMultBonus)}% Power
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}