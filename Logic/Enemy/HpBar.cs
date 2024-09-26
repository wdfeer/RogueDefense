using Godot;

namespace RogueDefense.Logic.Enemy;

public partial class HpBar : ProgressBar
{
    public Enemy enemy;
    public override void _Ready()
    {
        enemy = GetParent<Enemy>();
    }
    public override void _Process(double delta)
    {
        Value = enemy.Hp / enemy.maxHp;
        (GetNode("HpText") as Label).Text = $"{MathHelper.ToShortenedString(enemy.Hp)} / {MathHelper.ToShortenedString(enemy.maxHp)}";
    }
}