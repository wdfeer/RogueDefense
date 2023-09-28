using Godot;
using System;

public partial class Turret : Node2D
{
    public override void _Process(double delta)
    {
        if (IsInstanceValid(Enemy.instance))
        {
            (GetNode("Sprite2D") as Sprite2D).LookAt(Enemy.instance.GlobalPosition);
        }
    }
    public void SetLabel(string text)
    {
        Label label = (Label)GetNode("Label");
        label.Text = text;
    }
}
