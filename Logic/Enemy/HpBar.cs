using Godot;
using RogueDefense.Logic;
using System;

public partial class HpBar : ProgressBar
{
    public override void _Process(double delta)
    {
        Value = Enemy.instance.Hp / Enemy.instance.maxHp;
        (GetNode("HpText") as Label).Text = $"{MathHelper.ToShortenedString(Enemy.instance.Hp)} / {MathHelper.ToShortenedString(Enemy.instance.maxHp)}";
    }
}
