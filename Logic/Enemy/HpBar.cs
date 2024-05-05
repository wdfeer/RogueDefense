using Godot;
using RogueDefense.Logic;
using System;

public partial class HpBar : ProgressBar
{
    public Enemy enemy;
    public override void _Process(double delta)
    {
        Value = enemy.Hp / enemy.maxHp;
        (GetNode("HpText") as Label).Text = $"{MathHelper.ToShortenedString(enemy.Hp)} / {MathHelper.ToShortenedString(enemy.maxHp)}";
    }
}
