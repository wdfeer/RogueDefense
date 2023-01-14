using Godot;
using RogueDefense.Logic;
using System;

public class Bullet : MovingKinematicBody2D
{
	public override void _Process(float delta)
	{
		base._Process(delta);
	}

	public float damage = 1;
	protected override void OnCollision(KinematicCollision2D collision)
	{
		if (collision.Collider == Game.instance.enemy)
		{
			Game.instance.enemy.Damage(damage);
			QueueFree();
		}
	}
}
