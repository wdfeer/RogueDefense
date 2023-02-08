using Godot;
using RogueDefense.Logic;
using System;

public class HpBar : ProgressBar
{
    public override void _Process(float delta)
    {
        Value = Enemy.instance.Hp / Enemy.instance.maxHp;
        (GetNode("HpText") as Label).Text = $"{MathHelper.ToShortenedString(Enemy.instance.Hp)} / {MathHelper.ToShortenedString(Enemy.instance.maxHp)}";
    }
}
