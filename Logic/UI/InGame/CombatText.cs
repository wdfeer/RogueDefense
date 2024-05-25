using Godot;

public partial class CombatText : Label
{
    public Vector2 direction = new Vector2(0, -1);
    public const float SPEED = 120f;
    public const float LIFESPAN = 0.75f;
    public override void _Process(double delta)
    {
        SetPosition(Position + direction * (float)delta * SPEED);
        var mod = Modulate;
        mod.A -= (float)delta / LIFESPAN;
        if (mod.A <= 0f)
            QueueFree();
        Modulate = mod;
    }
}
