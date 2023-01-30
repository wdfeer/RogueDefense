using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class FuseBulletsAbility : ActiveAbility
    {
        public FuseBulletsAbility(CustomButton button) : base(button) { }
        public override void Activate()
        {
            PlayerShootManager shooter = Player.localInstance.shootManager;

            float hitMult = shooter.bullets.Aggregate(0f, (a, b) => a + (Object.IsInstanceValid(b) ? b.hitMult : 0));
            shooter.ClearBullets();

            Bullet bullet = shooter.Shoot(3f);
            bullet.SetHitMultiplier(hitMult * (1f + PowerMultBonus));
            bullet.damage = shooter.damage;
            bullet.Scale *= 2f;
        }
        public float PowerMultBonus => 2f * Strength;
        public override float BaseCooldown => 15f / Duration;
        protected override string GetAbilityText()
            => $@"Fuse all bullets on screen into one
with +{MathHelper.ToPercentAndRound(PowerMultBonus)}% Power
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}