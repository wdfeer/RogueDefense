using Godot;

namespace RogueDefense
{
    public class ArmorStripAbility : ActiveAbility
    {
        public ArmorStripAbility(CustomButton button) : base(button) { }
        public override void Activate()
        {
            if (Strip > 1f)
                Enemy.instance.armor = 0;
            else
                Enemy.instance.armor *= 1 - Strip;
        }
        public const float BASE_STRIP = 0.4f;
        public override float BaseCooldown => (NetworkManager.Singleplayer ? 25f : 40f) / Mathf.Sqrt(Duration);
        public float Strip => BASE_STRIP * Strength;
        protected override string GetAbilityText()
            => $@"Remove {(int)(Strip * 100f)}% Enemy Armor
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}