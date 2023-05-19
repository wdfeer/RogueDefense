using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public class CritChanceAbility : ActiveAbility
    {
        public CritChanceAbility(Player player, CustomButton button) : base(player, button)
        {
        }
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
                player.upgradeManager.critChance += CritChance;
            }
        }
        public float CritChance => Strength * (1f + player.upgradeManager.critChance);
        public override float BaseCooldown => 20f;
        protected override string GetAbilityText()
            => $@"+{MathHelper.ToPercentAndRound(CritChance)}% Crit Chance
Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}