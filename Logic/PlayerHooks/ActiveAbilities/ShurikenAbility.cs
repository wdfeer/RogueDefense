using Godot;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public partial class ShurikenAbility : ActiveAbility
    {
        public static PackedScene shurikenScene = (PackedScene)ResourceLoader.Load("res://Scenes/Shuriken.tscn");
        public int ShurikenCount => Mathf.FloorToInt(Strength / 1f);
        public override void Activate()
        {
            for (int i = 0; i < ShurikenCount; i++)
            {
                Shuriken proj = shurikenScene.Instantiate<Shuriken>();
                proj.owner = player;
                proj.velocity = new Vector2(20f, 0f).Rotated(0.1f * GD.Randf());
                proj.damage = Damage;
                player.shootManager.projectileManager.proj.Add(proj);
            }
        }
        public const float BASE_DAMAGE = 4;

        public ShurikenAbility(Player player, Button button) : base(player, button)
        {
            if (button != null)
            {
                button.Icon = (Texture2D)GD.Load("res://Assets/Images/game-icons.net/flying-shuriken.svg");
            }
        }

        public override bool Shared => false;
        public float Damage => BASE_DAMAGE * Strength * player.shootManager.damage;
        public override float BaseCooldown => 10f / Mathf.Sqrt(Duration);
        protected override string GetAbilityText()
            => $@"Throw {ShurikenCount} Shuriken{(ShurikenCount > 1 ? "s" : "")} with {(int)Damage} Damage
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}