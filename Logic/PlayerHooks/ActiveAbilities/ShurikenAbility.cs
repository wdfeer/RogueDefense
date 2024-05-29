using Godot;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense;

public partial class ShurikenAbility : ActiveAbility
{
    public int ShurikenCount => Mathf.FloorToInt(Strength / 0.8f);
    public override void Activate()
    {
        for (int i = 0; i < ShurikenCount; i++)
        {
            Shuriken proj = new Shuriken(player.shootManager.projectileManager.textures)
            {
                owner = player,
                velocity = new Vector2(20f, 0f).Rotated(0.1f * GD.Randf()),
                damage = Damage,
                position = DefenseObjective.instance.GlobalPosition
            };
            player.shootManager.projectileManager.proj.Add(proj);
        }
    }
    public const float BASE_DAMAGE = 5;

    public ShurikenAbility(Player player, Button button) : base(player, button)
    {
        if (button != null)
        {
            button.Icon = (Texture2D)GD.Load("res://Assets/Images/Icons/game-icons.net/flying-shuriken.svg");
        }
    }

    public override bool Shared => false;
    public float Damage => BASE_DAMAGE * Strength * player.shootManager.damage;
    public override float BaseCooldown => 10f / Mathf.Pow(Duration, 0.65f);
    protected override string GetAbilityText()
        => $@"Throw {ShurikenCount} Shuriken{(ShurikenCount > 1 ? "s" : "")} with {(int)Damage} Damage
Cooldown: {Cooldown:0.00} s";
}