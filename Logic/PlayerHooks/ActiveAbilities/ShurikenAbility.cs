using Godot;
namespace RogueDefense
{
    public class ShurikenAbility : ActiveAbility
    {
        public ShurikenAbility(CustomButton button) : base(button) { }

        public static PackedScene shurikenScene = (PackedScene)ResourceLoader.Load("res://Scenes/Shuriken.tscn");
        public override void Activate()
        {
            Shuriken proj = ((Shuriken)shurikenScene.Instance());
            proj.velocity = new Vector2(15f, 0f);
            proj.damage = BASE_DAMAGE * Strength;
            Player.AddChild(proj);
        }
        public const float BASE_DAMAGE = 7;
        public override float BaseCooldown => 12.5f;
        protected override string GetAbilityText()
            => $@"Throw a Shuriken with {(int)(10f * Strength)} Damage
Bleed Duration: {(5f * Duration).ToString("0.00")} s
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}