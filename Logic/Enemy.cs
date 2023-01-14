using Godot;
using RogueDefense.Logic;
using System;

public class Enemy : MovingKinematicBody2D
{
	public override void _Ready()
	{
		velocity = new Vector2(-1, 0);
		maxHp = 4 * Mathf.Pow(1 + Game.instance.generation / 4, 2);
		hp = maxHp;
	}

	public float maxHp;
	public float hp;
	public void Damage(float damage)
	{
		hp -= damage;
		if (hp <= 0)
		{
			Game.instance.DeleteEnemy();	
		}
	}
	public float damage = 1;
	protected override void OnCollision(KinematicCollision2D collision)
	{
		if (collision.Collider == Game.instance.player)
		{
			Game.instance.player.Damage(damage);
		}
	}
}
