using Godot;
using System;

public partial class BulletDeleter : Area2D
{
    public override void _Ready()
    {
        BodyExited += OnBodyExited;
    }
    public void OnBodyExited(Node body)
    {
        if (body is Bullet or EnemyBullet)
            body.QueueFree();
    }
}
