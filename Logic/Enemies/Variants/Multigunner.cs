using Godot;
using RogueDefense;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.Statuses;
using System;
using System.Linq;

public partial class Multigunner : Enemy
{
	[Export]
	PackedScene gunScene;
	[Export]
	PackedScene bulletScene;

	protected virtual int GunCount => Game.GetStage() + 1;
	public override void _Ready()
	{
		base._Ready();

		SpawnGuns(GunCount);
		GetNode<Sprite2D>("Sprite2D").Rotate(GD.Randf() * Mathf.Pi);
	}
	Node2D[] spawnPoints;
	void SpawnGuns(int count)
	{
		spawnPoints = new Node2D[count];
		for (int i = 0; i < count; i++)
		{
			Node2D gun = gunScene.Instantiate<Node2D>();
			GetNode<Sprite2D>("Sprite2D").AddChild(gun);
			gun.GlobalPosition = GlobalPosition;
			gun.Rotation = Mathf.Pi * 2 * i / count;
			spawnPoints[i] = gun.GetNode<Node2D>("SpawnPoint");
		}
	}


	public override float GetBaseSpeed()
		=> 0.4f;
	protected override bool ShieldOrbsAllowed => false;

	protected override void ModifyArmor(ref float armor)
	{
		armor += 100f;
	}


	protected virtual float HpCriticalModifier => (Hp / maxHp < 0.5f) ? 2 : 1;

	float RotationSpeed => MathF.PI / 3 * HpCriticalModifier;

	float shootTimer = GD.Randf();
	float ShootInterval => 1f / HpCriticalModifier;

	public override void _Process(double delta)
	{
		base._Process(delta);

		GetNode<Sprite2D>("Sprite2D").Rotate(RotationSpeed * (float)delta);

		shootTimer += (float)delta;
		if (shootTimer > ShootInterval)
		{
			shootTimer = 0;
			Shoot();
		}
	}



	float ShootSpeed => 100 * HpCriticalModifier;
	void Shoot()
	{
		for (int i = 0; i < spawnPoints.Length; i++)
		{
			EnemyBullet bullet = bulletScene.Instantiate<EnemyBullet>();
			AddSibling(bullet);

			Vector2 pos = spawnPoints[i].GlobalPosition;
			bullet.GlobalPosition = pos;

			Vector2 velocity = GlobalPosition.DirectionTo(pos) * ShootSpeed;
			bullet.velocity = velocity;

			bullet.damage = damage;
			bullet.lifespan = 10 / HpCriticalModifier;
		}
	}
}
