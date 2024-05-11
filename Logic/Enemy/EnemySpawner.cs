using Godot;
using System;
using System.Diagnostics.CodeAnalysis;

public partial class EnemySpawner : Node2D
{
	[Export]
	public PackedScene regularEnemyScene;
	[Export]
	public PackedScene armoredSpiritScene;
	[Export]
	public PackedScene miniArmoredSpiritScene;
	[Export]
	public PackedScene explodingEyeScene;

	Enemy InstantiateEnemy(int gen, int index)
	{
		Enemy InstantiateRandomNonBoss()
		{
			switch (Enemy.statsRng.RandiRange(0, 2))
			{
				case 0:
					return miniArmoredSpiritScene.Instantiate<Enemy>();
				case 1 when index != 0:
					return explodingEyeScene.Instantiate<Enemy>();
				default:
					return regularEnemyScene.Instantiate<Enemy>();
			}
		}

		switch (gen)
		{
			case 19:
				return armoredSpiritScene.Instantiate<Enemy>();
			case > 22:
				return InstantiateRandomNonBoss();
			default:
				return regularEnemyScene.Instantiate<Enemy>();
		}
	}
	int GetEnemyCount(int gen)
	{
		if (gen % 10 == 9)
			return 1;

		return Enemy.statsRng.RandiRange(1, 3) + Enemy.oneTimeCountIncrease;
	}
	Vector2 GetPositionOffset(int gen, int index)
	{
		switch (gen)
		{
			case 19:
				return Vector2.Zero;
			default:
				return new Vector2(Enemy.statsRng.RandiRange(0, 250), MathF.Sqrt(index) * 80 * (index % 2 == 0 ? 1 : -1));
		}
	}

	public void SpawnEnemies()
	{
		int count = GetEnemyCount(Game.Wave);

		for (int i = 0; i < count; i++)
		{
			Enemy enemy = InstantiateEnemy(Game.Wave, i);
			Enemy.enemies.Add(enemy);
			AddChild(enemy);
			enemy.Position = GetPositionOffset(Game.Wave, i);
		}

		Enemy.oneTimeCountIncrease = 0;
	}
}
