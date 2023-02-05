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
                proj.velocity = new Vector2(20f, 0f).Rotated(0.1f * GD.Randf());
                proj.damage = Damage;
                Player.localInstance.AddChild(proj);
                Player.shootManager.bullets.Add(proj);
            }
        }
        public const float BASE_DAMAGE = 4;
        public float Damage => BASE_DAMAGE * Strength * Player.shootManager.damage;
        public override float BaseCooldown => 12f / Mathf.Pow(1f + (Duration - 1f) * 0.75f, 0.65f);
        protected override string GetAbilityText()
            => $@"Throw {ShurikenCount} Shuriken{(ShurikenCount > 1 ? "s" : "")} with {(int)(Damage)} Damage
Cooldown: {Cooldown.ToString("0.00")} s";
    }
}