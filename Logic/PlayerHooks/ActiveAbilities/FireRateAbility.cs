namespace RogueDefense
{
    public class FireRateAbility : ActiveAbility
    {
        public FireRateAbility(CustomButton button) : base(button) { }
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
                Player.shootManager.shootInterval /= 1 + 0.75f * Strength;
                Player.shootManager.shootSpeed *= 2;
            }
        }
        public override float BaseCooldown => 22f;
        protected override string GetAbilityText()
            => $@"+{(int)(75f * Strength)}% Total Fire Rate and
+100% Projectile velocity
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}