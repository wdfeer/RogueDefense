using Godot;
using System;

public class CombatText : Label
{
    public const float SPEED = 80f;
    public const float LIFESPAN = 0.75f;
    public override void _Process(float delta)
    {
        SetPosition(RectPosition + new Vector2(0, delta * SPEED * -1));
        var mod = Modulate;
        mod.a -= delta / LIFESPAN;
        if (mod.a <= 0f)
            QueueFree();
        Modulate = mod;
    }
}
