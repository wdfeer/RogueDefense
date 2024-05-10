using Godot;
using RogueDefense;
using RogueDefense.Logic;
using System;

public partial class EnemyBullet : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public Vector2 velocity;
	public override void _PhysicsProcess(double delta)
	{
		Position += velocity * (float)delta;
	}

	public float damage = 20f;
	void OnBodyEntered(Node body)
	{
		if (body is DefenseObjective defenseObjective)
		{
			defenseObjective.Damage(damage);
		}
	}
}
