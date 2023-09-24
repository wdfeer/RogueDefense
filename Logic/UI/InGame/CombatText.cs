using Godot;
using System;

public partial class CombatText : Label
{
    public Vector2 direction = new Vector2(0, -1);
    public const float SPEED = 120f;
    public const float LIFESPAN = 0.75f;
    public override void _Process(float delta)
    {
        SetPosition(Position + direction * delta * SPEED);
        var mod = Modulate;
        mod.a -= delta / LIFESPAN;
        if (mod.a <= 0f)
            QueueFree();
        Modulate = mod;
    }
}
