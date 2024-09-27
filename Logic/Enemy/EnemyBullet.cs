using System;

namespace RogueDefense.Logic.Enemy;
public partial class EnemyBullet : Area2D
{
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public float lifespan = 5;
	public Vector2 velocity;
	public override void _PhysicsProcess(double delta)
	{
		Position += velocity * (float)delta;

		lifespan -= (float)delta;
		Modulate = Modulate with { A = MathF.Min(lifespan, 1f) };
		if (lifespan < 0)
			QueueFree();
	}

	public float damage = 20f;
	const float STUN_DURATION = 4;
	void OnBodyEntered(Node body)
	{
		if (body is DefenseObjective defenseObjective)
		{
			defenseObjective.Damage(damage);
			QueueFree();
		}
		else if (body is Player.Turret.Turret turret && !turret.Stunned)
		{
			turret.Stun(STUN_DURATION);
			QueueFree();
		}
	}
}
