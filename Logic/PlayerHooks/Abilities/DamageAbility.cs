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
        public override void PostUpdate(float delta)
        {
            if (buffLeft > 0)
            {
                buffLeft -= delta;
                Player.shootManager.damage *= 1f + 1f * Strength;
            }
        }

        protected override string GetAbilityText()
            => $@"+{(int)(100f * Strength)}% Total Damage
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}