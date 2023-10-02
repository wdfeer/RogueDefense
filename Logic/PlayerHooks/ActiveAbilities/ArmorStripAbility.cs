using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class ArmorStripAbility : ActiveAbility
    {
        public ArmorStripAbility(Player player, CustomButton button) : base(player, button)
        {
        }
        public override void Activate()
        {
            if (Strip > 1f)
            {
                Enemy.instance.armor = 0;
                foreach (var status in Enemy.instance.statuses)
                {
                    status.immune = false;
                }
            }
            else
                Enemy.instance.armor *= 1 - Strip;
        }
        public const float BASE_STRIP = 0.3f;



        public override float BaseCooldown => (NetworkManager.Singleplayer ? 30f : 45f) / Mathf.Sqrt(Duration);
        public float Strip => BASE_STRIP * Strength;
        protected override string GetAbilityText()
        {
            string str = $"Remove {(Strip > 1f ? 100 : MathHelper.ToPercentAndRound(Strip))}% Enemy Armor\n";
            if (Strip > 1f)
                str += "and remove all Enemy Immunities\n";
            str += $"Cooldown: {Cooldown.ToString("0.00")} s";
            return str;
        }
    }
}