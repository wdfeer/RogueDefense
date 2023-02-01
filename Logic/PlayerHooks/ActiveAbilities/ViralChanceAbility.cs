using RogueDefense.Logic;

namespace RogueDefense
{
    public class ViralChanceAbility : ActiveAbility
    {
        public ViralChanceAbility(CustomButton button) : base(button) { }
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
                Player.upgradeManager.viralChance += FlatBonus;
                Player.upgradeManager.viralChance *= 1 + MultBonus;
            }
        }

        public float FlatBonus => 0.25f * Strength;
        public float MultBonus => 0.5f * Strength;
        public override float BaseCooldown => 20f;
        protected override string GetAbilityText()
            => $@"+{MathHelper.ToPercentAndRound(FlatBonus)}% Viral Chance and then
+{MathHelper.ToPercentAndRound(MultBonus)}% Total Viral Chance
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}