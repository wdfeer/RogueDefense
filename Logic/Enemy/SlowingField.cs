using Godot;
using System;

public class SlowingField : Area2D
{
    public void Enable()
    {
        Visible = true;
        Connect("body_entered", this, "BodyEntered");
        ((CircleShape2D)(GetNode("CollisionShape2D") as CollisionShape2D).Shape).Radius = radius;
    }
    public void BodyEntered(Node body)
    {
        if (body is Bullet)
        {
            Bullet bullet = (Bullet)body;
            bullet.velocity *= 0.05f;
        }
    }

    public float radius = 190;
    public override void _Draw()
    {
        DrawCircle(Position, radius, Color.Color8(0, 0, 25, 100));
    }
}
