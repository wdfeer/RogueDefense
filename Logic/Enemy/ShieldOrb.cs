using Godot;
using RogueDefense;
using System;

public partial class ShieldOrb : Area2D
{
	public static float damageConsumed = 0f;
	ShieldOrbButton button;
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		button = (ShieldOrbButton)GetNode("Button");
	}
	public void OnBodyEntered(Node body)
	{
		if (body is Bullet bullet)
		{
			bullet.ShieldOrbCollision(this);
		}
		else if (body is DefenseObjective defObjective && !tappable)
		{
			defObjective.Damage(10f * Game.GetStage());
		}
	}

	bool tappable = true;
	public void SetTappability(bool tappable)
	{
		this.tappable = tappable;

		((Label)GetNode("Label")).Visible = tappable;

		button.Disabled = !tappable;
		UpdateSpriteModulate();
	}
	void UpdateSpriteModulate()
		=> button.Modulate = (tappable && !exploding) ? Colors.White : Colors.Red;

	bool exploding = false;
	public void SetExploding(bool exploding)
	{
		this.exploding = exploding;

		UpdateSpriteModulate();
	}
	[Export]
	public PackedScene enemyBullet;
	public void TryExplode()
	{
		if (!exploding)
			return;

		CallDeferred("SpawnBullets");
	}
	void SpawnBullets()
	{
		for (int i = 0; i < 5; i++)
		{
			EnemyBullet bullet = enemyBullet.Instantiate<EnemyBullet>();
			AddSibling(bullet);

			bullet.velocity = new Vector2(GD.Randf() - 0.5f, GD.Randf() - 0.5f).Normalized() * 200f;
		}
	}
}
