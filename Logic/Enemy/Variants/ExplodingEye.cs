using Godot;
using System;

public partial class ExplodingEye : Enemy
{
	public override float GetBaseSpeed() => 2f;
	protected override void ModifyMaxHp(ref float maxHp)
	{
		maxHp /= 3f;
	}
	protected override void ModifyArmor(ref float armor)
	{
		armor = 0;
	}
	protected override bool ShieldOrbsAllowed => false;

	[Export]
	public PackedScene bulletScene;
	protected override void OnDeath()
	{
		CallDeferred("SpawnBullets");
	}
	void SpawnBullets()
	{
		float angle = GD.Randf() * MathF.PI;
		int count = 8;
		for (int i = 0; i < count; i++)
		{
			EnemyBullet bullet = bulletScene.Instantiate<EnemyBullet>();
			AddSibling(bullet);
			bullet.Position = Position;
			bullet.damage = damage / 2;

			bullet.velocity = new Vector2(MathF.Sin(angle), MathF.Cos(angle)) * 125f;
			angle += MathF.Tau / count;
		}
	}
}
