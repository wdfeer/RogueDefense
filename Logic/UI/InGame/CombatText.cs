using Godot;
using System;

public class CombatText : Label
{
    public Vector2 direction = new Vector2(0, -1);
    public const float SPEED = 300f;
    public const float LIFESPAN = 0.25f;
    public override void _Process(float delta)
    {
        SetPosition(RectPosition + direction * delta * SPEED);
        var mod = Modulate;
        mod.a -= delta / LIFESPAN;
        if (mod.a <= 0f)
            QueueFree();
        Modulate = mod;
    }
}
