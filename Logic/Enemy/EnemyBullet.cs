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

	void OnBodyEntered(Node body)
	{
		if (body is DefenseObjective defenseObjective)
		{
			defenseObjective.Damage(5f * Game.instance.GetStage());
		}
	}
}
