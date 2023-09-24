using Godot;
using System;

public partial class PlayArea : Area2D
{
    public override void _Ready()
    {
        Connect("body_exited", new Callable(this, "BodyExited"));
    }
    public void BodyExited(Node body)
    {
        if (body is Bullet)
            body.QueueFree();
    }
}
