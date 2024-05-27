using Godot;
using RogueDefense.Logic.Enemies;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	public PackedScene regularEnemyScene;
	[Export]
	public PackedScene firstBossScene;
	[Export]
	public PackedScene armoredSpiritBossScene;
	[Export]
	public PackedScene miniArmoredSpiritScene;
	[Export]
	public PackedScene explodingEyeScene;

	Enemy InstantiateEnemy(int gen, int index)
	{
		return explodingEyeScene.Instantiate<Enemy>(); // DEBUG
		Enemy InstantiateRandomNormal()
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
		Enemy InstantiateRandomBoss()
		{
			switch (Enemy.statsRng.RandiRange(0, 1))
			{
				case 0:
					return armoredSpiritBossScene.Instantiate<Enemy>();
				default:
					return firstBossScene.Instantiate<Enemy>();
			}
		}

		switch (gen)
		{
			case 9:
				return firstBossScene.Instantiate<Enemy>();
			case 19:
				return armoredSpiritBossScene.Instantiate<Enemy>();
			case > 19 when gen % 10 == 9:
				return InstantiateRandomBoss();
			case > 19:
				return InstantiateRandomNormal();
			default:
				return regularEnemyScene.Instantiate<Enemy>();
		}
	}
	int GetEnemyCount(int gen)
	{
		if (gen % 10 == 9)
			return 1;

		return Enemy.statsRng.RandiRange(1, 3) + gen / 20 + Enemy.oneTimeCountIncrease;
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
