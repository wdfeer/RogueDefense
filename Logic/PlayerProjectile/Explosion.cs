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
	protected override int Radius => 100;
	public override void Draw(CanvasItem drawer)
	{
		Rect2 rect = new Rect2() { Position = position - new Vector2(Radius, Radius), Size = new Vector2(Diameter, Diameter) };
		drawer.DrawTextureRect(texture, rect, false, Colors.White with { A = timeLeft });
	}


	protected override void CheckCollision()
	{
		// Default collision detection doesn't work for some reason

		for (int i = 0; i < Enemy.enemies.Count; i++)
		{
			Enemy enemy = Enemy.enemies[i];
			if (!GodotObject.IsInstanceValid(enemy) || enemy.Dead)
				continue;

			if (enemy.GlobalPosition.DistanceTo(position) < Radius)
				EnemyCollision(enemy, enemy.GetRid());
		}
	}
}
