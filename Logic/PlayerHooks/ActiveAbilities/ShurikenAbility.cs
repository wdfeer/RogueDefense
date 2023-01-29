using Godot;
namespace RogueDefense
{
    public class ShurikenAbility : ActiveAbility
    {
        public ShurikenAbility(CustomButton button) : base(button) { }

        public static PackedScene shurikenScene = (PackedScene)ResourceLoader.Load("res://Scenes/Shuriken.tscn");
        public int ShurikenCount => Mathf.FloorToInt(Strength / 1f);
        public override void Activate()
        {
            for (int i = 0; i < ShurikenCount; i++)
            {
                Shuriken proj = ((Shuriken)shurikenScene.Instance());
                proj.velocity = new Vector2(15f, 0f).Rotated(0.1f * GD.Randf());
                proj.damage = BASE_DAMAGE * Strength;
                Player.AddChild(proj);
            }
        }
        public const float BASE_DAMAGE = 7;
        public override float BaseCooldown => 10f / Duration;
        protected override string GetAbilityText()
            => $@"Throw {ShurikenCount} Shuriken{(ShurikenCount > 1 ? "s" : "")} with {(int)(BASE_DAMAGE * Strength)} Damage
Bleed Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}