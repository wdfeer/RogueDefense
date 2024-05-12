using Godot;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class DamageAbility : ActiveAbility
    {
        public override void Activate()
        {
            buffLeft = Duration * 5f;
        }
        public float buffLeft = 0;

        public DamageAbility(Player player, Button button) : base(player, button)
        {
        }

        public override void PostUpgradeUpdate(float delta)
        {
            if (buffLeft > 0)
            {
                buffLeft -= delta;
                player.shootManager.shootSpeed *= 2;
            }
        }
        public override void PreShoot(ShootManager shooter)
        {
            if (buffLeft > 0)
            {
                shooter.damage *= 1f + 1f * Strength;
                shooter.EnableParticles();
            }
        }

        protected override string GetAbilityText()
            => $@"+{(int)(100f * Strength)}% Total Damage and
+100% Projectile velocity
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}