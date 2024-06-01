using Godot;
using Godot.Collections;
using RogueDefense.Logic;
using RogueDefense.Logic.Enemies;

public class Explosion : Projectile
{
	public Explosion(Array<Texture2D> textures) : base()
	{
		texture = textures[2];

		penetration = 16;
	}

	public float timeLeft = 0.5f;
	public override void PhysicsProcess(float delta)
	{
		base.PhysicsProcess(delta);

		timeLeft -= delta;
		if (timeLeft < 0)
			QueueFree();
	}

	Texture2D texture;
	public float radius = 100;
	protected override int Radius => (int)radius;
	public override void Draw(CanvasItem drawer)
	{
		Rect2 rect = new Rect2() { Position = position - new Vector2(Radius, Radius), Size = new Vector2(Diameter, Diameter) };
		drawer.DrawTextureRect(texture, rect, false, Colors.White with { A = timeLeft });
	}
}
