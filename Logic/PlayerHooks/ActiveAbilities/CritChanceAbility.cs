using RogueDefense.Logic;

namespace RogueDefense
{
    public class CritChanceAbility : ActiveAbility
    {
        public CritChanceAbility(CustomButton button) : base(button) { }
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
                Player.upgradeManager.critChance += CritChance;
            }
        }
        public float CritChance => Strength * (1f + Player.upgradeManager.critChance);
        public override float BaseCooldown => 20f;
        protected override string GetAbilityText()
            => $@"+{MathHelper.ToPercentAndRound(CritChance)}% Crit Chance
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}