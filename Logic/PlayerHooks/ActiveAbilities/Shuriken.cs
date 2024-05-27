using Godot;
using Godot.Collections;
using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;

namespace RogueDefense;

public partial class Shuriken : Projectile
{
	public Shuriken(Array<Texture2D> textures) : base()
	{
		texture = textures[1];
	}

	protected override int Radius => 16;
	readonly Texture2D texture;
	public override void Draw(CanvasItem drawer)
	{
		Rect2 rect = new Rect2() { Position = position - new Vector2(Radius, Radius), Size = new Vector2(Diameter, Diameter) };
		drawer.DrawTextureRect(texture, rect, false);
	}

	public override bool KillShieldOrbs => true;
	protected override void OnHit(Enemy enemy, float totalDmg)
	{
		enemy.AddBleed(totalDmg, 5f);
	}
	protected override bool UnhideableDamageNumbers => true;
}
