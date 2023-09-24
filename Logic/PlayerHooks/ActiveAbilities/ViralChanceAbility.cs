using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class ViralChanceAbility : ActiveAbility
    {
        public override void Activate()
        {
            buffLeft = Duration * BASE_DURATION;
        }
        public float buffLeft = 0;
        public override void PostUpgradeUpdate(float delta)
        {
            if (buffLeft > 0)
            {
                buffLeft -= delta;
                player.upgradeManager.viralChance += FlatBonus;
                player.upgradeManager.viralChance *= 1 + MultBonus;
                Enemy.instance.viral.immune = false;
            }
        }
        public const float BASE_DURATION = 4f;

        public ViralChanceAbility(Player player, CustomButton button) : base(player, button)
        {
        }

        public float FlatBonus => 0.2f * Strength;
        public float MultBonus => 1f * Strength;
        public override float BaseCooldown => 18f;
        protected override string GetAbilityText()
            => $@"+{MathHelper.ToPercentAndRound(FlatBonus)}% Viral Chance and then
+{MathHelper.ToPercentAndRound(MultBonus)}% Total Viral Chance
Duration: {(BASE_DURATION * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}