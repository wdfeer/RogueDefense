using Godot;
using System;

public class ArmorBar : TextureProgress
{
    public static ArmorBar instance;
    public override void _Ready()
    {
        instance = this;
    }

    public void SetDisplay(float damageReduction)
    {
        if (damageReduction <= 0.01f)
            Hide();
        else
            Show();

        double range = MaxValue - MinValue;
        Value = damageReduction * range + MinValue;
    }
}
