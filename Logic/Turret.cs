using Godot;
using System;

public class Turret : Node2D
{
    public override void _Process(float delta)
    {
        if (IsInstanceValid(Enemy.instance))
        {
            LookAt(Enemy.instance.GlobalPosition);
        }
    }
}
