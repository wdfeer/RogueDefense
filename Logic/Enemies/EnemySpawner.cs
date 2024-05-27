using Godot;
using RogueDefense.Logic.Enemies;
using System;
using System.Collections.Generic;

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
	[Export]
	public PackedScene multigunnerScene;

	Enemy InstantiateEnemy(int gen, int index)
	{
		Enemy InstantiateRandomNormal()
		{
			List<PackedScene> possibilities = new List<PackedScene>
			{
				regularEnemyScene,
				multigunnerScene
			};
			if (gen > 15)
				possibilities.Add(miniArmoredSpiritScene);
			if (gen > 19 && index != 0)
				possibilities.Add(explodingEyeScene);

			return possibilities[Enemy.statsRng.RandiRange(0, possibilities.Count - 1)].Instantiate<Enemy>();
		}
		Enemy InstantiateRandomBoss()
		{
			List<PackedScene> possibilities = new List<PackedScene>
			{
				firstBossScene,
				multigunnerScene
			};
			if (gen >= 19)
				possibilities.Add(armoredSpiritBossScene);

			return possibilities[Enemy.statsRng.RandiRange(0, possibilities.Count - 1)].Instantiate<Enemy>();
		}

		if (gen > 19 && gen % 10 == 9)
			return InstantiateRandomBoss();
		return InstantiateRandomNormal();
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