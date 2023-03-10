using Godot;
using System;

public class ShieldOrb : Area2D
{
    public static float damageConsumed = 0f;

    public override void _Ready()
    {
        Connect("body_entered", this, "BodyEntered");
    }
    public void BodyEntered(Node body)
    {
        if (body is Bullet bullet)
        {
            bullet.ShieldOrbCollision(this);
        }
    }
}
