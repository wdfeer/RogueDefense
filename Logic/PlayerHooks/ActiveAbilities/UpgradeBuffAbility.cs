using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class UpgradeBuffAbility : ActiveAbility
    {
        public UpgradeBuffAbility(Player player, Button button) : base(player, button)
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