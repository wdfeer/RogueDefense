namespace RogueDefense
{
    public class ArmorStripAbility : ActiveAbility
    {
        public ArmorStripAbility(CustomButton button) : base(button) { }
        public override void Activate()
        {
            buffLeft = Duration * BASE_DURATION;
            oldArmor = Enemy.instance.armor;
            if (Strip > 1f)
                Enemy.instance.armor = 0;
            else
                Enemy.instance.armor *= 1 - Strip;
            Enemy.instance.ResetArmorDisplay();
        }
        public float oldArmor;
        public float buffLeft = 0;
        public override void PostUpgradeUpdate(float delta)
        {
            if (buffLeft > 0)
            {
                buffLeft -= delta;
                if (buffLeft <= 0)
                {
                    Enemy.instance.armor = oldArmor;
                    Enemy.instance.ResetArmorDisplay();
                }
            }
        }
        public const float BASE_STRIP = 0.4f;
        public const float BASE_DURATION = 7.5f;
        public float Strip => BASE_STRIP * Strength;
        protected override string GetAbilityText()
            => $@"Remove {(int)(Strip * 100f)}% Enemy Armor
Duration: {(BASE_DURATION * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}