using Godot;
using System;

public partial class EffectField : Area2D
{
    public EffectFieldMode mode;
    public void Enable(EffectFieldMode mode)
    {
        this.mode = mode;
        Visible = true;
        Monitoring = true;
        Monitorable = true;

        Connect("body_entered", new Callable(this, "BodyEntered"));
        ((CircleShape2D)(GetNode("CollisionShape2D") as CollisionShape2D).Shape).Radius = radius;
    }
    public void BodyEntered(Node body)
    {
        if (!(body is Bullet))
            return;
        Bullet bullet = (Bullet)body;
        if (bullet.fused)
            return;
        switch (mode)
        {
            case EffectFieldMode.Slow:
                bullet.velocity *= 0.05f;
                return;
            case EffectFieldMode.Diffuse:
                bullet.velocity = bullet.velocity.Rotated((GD.Randf() - 0.5f) * 1.75f);
                return;
            default: return;
        }
    }

    public float radius = 190;
    public override void _Draw()
    {
        DrawCircle(Position, radius, Color.Color8(0, 0, 25, 100));
    }


    public enum EffectFieldMode
    {
        Slow,
        Diffuse
    }
}
