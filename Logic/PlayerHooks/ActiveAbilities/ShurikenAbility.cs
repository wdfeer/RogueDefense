using Godot;
namespace RogueDefense
{
    public class ShurikenAbility : ActiveAbility
    {

        public static PackedScene shurikenScene = (PackedScene)ResourceLoader.Load("res://Scenes/Shuriken.tscn");
        public int ShurikenCount => Mathf.FloorToInt(Strength / 1f);
        public override void Activate()
        {
            for (int i = 0; i < ShurikenCount; i++)
            {
                Shuriken proj = ((Shuriken)shurikenScene.Instance());
                proj.velocity = new Vector2(20f, 0f).Rotated(0.1f * GD.Randf());
                proj.damage = Damage;
                DefenseObjective.instance.AddChild(proj);
                player.shootManager.bullets.Add(proj);
            }
        }
        public const float BASE_DAMAGE = 4;

        public ShurikenAbility(Player player, CustomButton button) : base(player, button)
        {
        }

        public float Damage => BASE_DAMAGE * Strength * player.shootManager.damage;
        public override float BaseCooldown => 10f / Mathf.Sqrt(1f + (Duration - 1f) * 0.75f);
        protected override string GetAbilityText()
            => $@"Throw {ShurikenCount} Shuriken{(ShurikenCount > 1 ? "s" : "")} with {(int)(Damage)} Damage
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}