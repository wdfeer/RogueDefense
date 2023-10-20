using Godot;
using System;

public partial class PlayArea : Area2D
{
    public override void _Ready()
    {
        BodyExited += OnBodyExited;
    }
    public void OnBodyExited(Node body)
    {
        if (body is Bullet)
            body.QueueFree();
    }
}
