using Godot;
using RogueDefense.Logic;
using System;

public class Enemy : MovingKinematicBody2D
{
	public override void _Ready()
	{
		velocity = new Vector2(-1, 0);
		maxHp = 10 * (1 + Game.instance.generation / 3);
		hp = maxHp;
	}

	public int maxHp = 10;
	public int hp;
	public void Damage(int damage)
	{
		hp -= damage;
		if (hp <= 0)
		{
			Game.instance.DeleteEnemy();	
		}
	}
	public int damage = 1;
	protected override void OnCollision(KinematicCollision2D collision)
	{
		if (collision.Collider == Game.instance.player)
		{
			Game.instance.player.Damage(damage);
		}
	}
}
