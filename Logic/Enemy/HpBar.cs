using Godot;
using System;

public class HpBar : ProgressBar
{
    public override void _Process(float delta)
    {
        Value = Enemy.instance.Hp / Enemy.instance.maxHp;
        (GetNode("HpText") as Label).Text = $"{ShortenNumber(Enemy.instance.Hp)} / {ShortenNumber(Enemy.instance.maxHp)}";
    }
    public static string ShortenNumber(float x)
    {
        if (x < 1000)
            return x.ToString("0.0");
        else if (x < 1000000)
            return (x / 1000f).ToString("0.0") + "K";
        else
            return (x / 1000000f).ToString("0.0") + "M";
    }
}
