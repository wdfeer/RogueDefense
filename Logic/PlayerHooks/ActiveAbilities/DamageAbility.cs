namespace RogueDefense
{
    public class DamageAbility : ActiveAbility
    {
        public DamageAbility(CustomButton button) : base(button) { }
        public override void Activate()
        {
            buffLeft = Duration * 5f;
        }
        public float buffLeft = 0;
        public override void PostUpgradeUpdate(float delta)
        {
            if (buffLeft > 0)
            {
                buffLeft -= delta;
                Player.shootManager.shootSpeed *= 2;
            }
        }
        public override void PostShoot(Bullet bullet)
        {
            if (buffLeft > 0)
            {
                bullet.damage *= 1f + 1f * Strength;
                bullet.StartParticleEffect();
            }
        }

        protected override string GetAbilityText()
            => $@"+{(int)(100f * Strength)}% Total Damage and
+100% Projectile velocity
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}