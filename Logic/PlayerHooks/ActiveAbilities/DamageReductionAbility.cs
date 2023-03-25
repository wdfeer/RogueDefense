using RogueDefense.Logic;

namespace RogueDefense
{
    public class DamageReductionAbility : ActiveAbility
    {
        public DamageReductionAbility(CustomButton button) : base(button) { }
        public override void Activate()
        {
            buffLeft = Duration * 7.5f;
        }
        public float buffLeft = 0;
        public override void PostUpgradeUpdate(float delta)
        {
            if (buffLeft > 0)
            {
                buffLeft -= delta;
                DefenseObjective.instance.damageMult *= DamageTakenMult;
            }
        }
        public float DamageTakenMult => 0.5f / Strength;
        public float DamageReduction => 1f - DamageTakenMult;
        protected override string GetAbilityText()
            => $@"+{(DamageReduction * 100f).ToString("0.00")}% Damage Reduction
Duration: {(7.5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}