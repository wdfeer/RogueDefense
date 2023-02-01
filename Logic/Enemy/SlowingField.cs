using Godot;
using System;

public class SlowingField : Area2D
{
    public void Enable()
    {
        Visible = true;
        Connect("body_entered", this, "BodyEntered");
    }
    public void BodyEntered(Node body)
    {
        if (body is Bullet)
        {
            Bullet bullet = (Bullet)body;
            bullet.velocity *= 0.05f;
        }
    }
}
