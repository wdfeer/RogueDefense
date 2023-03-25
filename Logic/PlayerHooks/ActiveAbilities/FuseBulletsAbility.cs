using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class FuseBulletsAbility : ActiveAbility
    {
        public FuseBulletsAbility(CustomButton button) : base(button) { }
        public override bool CanBeActivated()
            => Player.local.shootManager.bullets.Any(x => Object.IsInstanceValid(x) && !x.fused);
        public override void Activate()
        {
            ShootManager shooter = Player.local.shootManager;

            float hitMult = shooter.bullets.Aggregate(0f, (a, b) =>
                a + ((Object.IsInstanceValid(b) && !b.fused) ? b.hitMult : 0));
            shooter.ClearBullets(x => !x.fused);
            Bullet bullet = shooter.Shoot(Player.shootManager.bulletSpawns[0], 3f);
            bullet.SetHitMultiplier(hitMult * (1f + PowerMultBonus));
            bullet.damage = shooter.damage;
            bullet.Scale *= 2f;
            bullet.StartParticleEffect();
            bullet.fused = true;
        }
        public float PowerMultBonus => 1.5f * Strength;
        public override float BaseCooldown => 10f / Mathf.Sqrt(Duration);
        protected override string GetAbilityText()
            => $@"Fuse all bullets on screen into one
with +{MathHelper.ToPercentAndRound(PowerMultBonus)}% Power
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}