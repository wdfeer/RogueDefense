using Godot;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class ArmorStripAbility : ActiveAbility
    {
        public ArmorStripAbility(Player player, Button button) : base(player, button)
        {
        }
        public override void Activate()
        {
            foreach (Enemy enemy in Enemy.enemies)
            {
                ArmorStrip(enemy);
            }
        }
        void ArmorStrip(Enemy enemy)
        {
            if (Strip > 1f)
            {
                enemy.armor = 0;
                foreach (var status in enemy.statuses)
                {
                    status.immune = false;
                }
            }
            else
                enemy.armor *= 1 - Strip;
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