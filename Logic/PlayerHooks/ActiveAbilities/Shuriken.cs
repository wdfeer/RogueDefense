using System;
using Godot;
using Godot.Collections;
using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;

namespace RogueDefense;

public class Shuriken : Projectile
{
	public Shuriken(Array<Texture2D> textures) : base()
	{
		texture = textures[1];
	}

	protected override int Radius => 16;
	readonly Texture2D texture;
	const float ROTATION_SPEED = Mathf.Pi * 3;
	float rotation = 0;
	public override void Draw(CanvasItem drawer)
	{
		drawer.DrawSetTransform(position, rotation);

		Rect2 rect = new Rect2() { Position = -new Vector2(Radius, Radius), Size = new Vector2(Diameter, Diameter) };
		drawer.DrawTextureRect(texture, rect, false);
	}
	public override void PhysicsProcess(float delta)
	{
		base.PhysicsProcess(delta);

		rotation += delta * ROTATION_SPEED;
	}

	public override bool KillShieldOrbs => true;
	protected override void OnHit(Enemy enemy, float totalDmg)
	{
		enemy.AddBleed(totalDmg, 5f);
	}
	protected override bool UnhideableDamageNumbers => true;
}
