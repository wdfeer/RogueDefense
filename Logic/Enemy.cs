using Godot;
using RogueDefense.Logic;
using System;

public class Enemy : MovingKinematicBody2D
{
	public override void _Ready()
	{
		velocity = new Vector2(-1.1f, 0);
		maxHp = 6f * Mathf.Pow(1f + Game.instance.generation * 0.5f, 1.5f);
		Hp = maxHp;
	}

	public float maxHp;
	private float hp;
	public float Hp
	{
		get => hp; set
		{
			hp = value;
			(GetNode("./HpBar") as ProgressBar).Value = hp / maxHp;
		}
	}
	public void Damage(float damage)
	{
		Hp -= damage;
		if (Hp <= 0)
		{
			Game.instance.DeleteEnemy();	
		}
	}
	public float dps = 20;
	public override void _Process(float delta)
	{
		base._Process(delta);
		if (attacking)
			Game.instance.player.Damage(dps * delta);
	}
	bool attacking = false;

	protected override void OnCollision(KinematicCollision2D collision)
	{
		if (collision.Collider == Game.instance.player)
		{
			attacking = true;
		}
	}
}
