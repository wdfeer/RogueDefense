using RogueDefense.Logic;

namespace RogueDefense
{
    public class UpgradeBuffAbility : ActiveAbility
    {
        public UpgradeBuffAbility(CustomButton button) : base(button)
        {
            active = false;
        }
        public override void Activate()
        {
            active = true;
        }
        public static bool active = false;
        public const float UPGRADE_VALUE_INCREASE = 1f;
        public override float BaseCooldown => 30f;
        public override bool ConstantValues => true;
        protected override string GetAbilityText()
            => $@"+{MathHelper.ToPercentAndRound(UPGRADE_VALUE_INCREASE)}% Upgrade value on next upgrades
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}