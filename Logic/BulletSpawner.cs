using Godot;
using RogueDefense.Logic.PlayerCore;
using System;

public partial class BulletSpawner : Node
{
	public static BulletSpawner instance;
	public override void _Ready()
	{
		instance = this;
	}

	[Export]
	private PackedScene bulletScene;

	public Bullet InstantiateBullet(Vector2 gposition)
	{
		Bullet bullet = bulletScene.Instantiate<Bullet>();
		AddChild(bullet);
		bullet.GlobalPosition = gposition;
		return bullet;
	}
}
