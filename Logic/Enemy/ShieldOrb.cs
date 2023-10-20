using Godot;
using System;

public partial class ShieldOrb : Area2D
{
	public static float damageConsumed = 0f;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}
	public void OnBodyEntered(Node body)
	{
		if (body is Bullet bullet)
		{
			bullet.ShieldOrbCollision(this);
		}
	}
}
