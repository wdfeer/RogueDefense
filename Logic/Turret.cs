using Godot;
using System;

public partial class Turret : Node2D
{
	public const int SPEED = 3;
	[Export]
	public AnimationPlayer animationPlayer;
	[Export]
	public Node2D bulletSpawnpoint;
	public override void _Process(double delta)
	{
		if (IsInstanceValid(Enemy.instance))
		{
			(GetNode("TurretSprite") as Sprite2D).LookAt(Enemy.instance.GlobalPosition);
		}
	}
	public void SetLabel(string text)
	{
		Label label = (Label)GetNode("Label");
		label.Text = text;
	}
}
