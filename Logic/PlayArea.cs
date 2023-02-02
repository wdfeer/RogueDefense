using Godot;
using System;

public class PlayArea : Area2D
{
    public override void _Ready()
    {
        Connect("body_exited", this, "BodyExited");
    }
    public void BodyExited(Node body)
    {
        if (body is Bullet)
            body.QueueFree();
    }
}
