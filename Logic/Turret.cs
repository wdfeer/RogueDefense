using Godot;
using System;

public class Turret : Node2D
{
    public override void _Process(float delta)
    {
        if (IsInstanceValid(Enemy.instance))
        {
            (GetNode("Sprite") as Sprite).LookAt(Enemy.instance.GlobalPosition);
        }
    }
    public void SetLabel(string text)
    {
        Label label = (Label)GetNode("Label");
        label.Text = text;
    }
}
