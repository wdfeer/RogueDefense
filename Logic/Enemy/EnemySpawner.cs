using System;
using System.Collections.Generic;

namespace RogueDefense.Logic.Enemy;

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
	[Export]
	public PackedScene multigunnerBossScene;
	[Export]
	public PackedScene arcaneBossScene;
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
		Enemy InstantiateBoss()
		{
			switch (gen)
			{
				case 9:
					return firstBossScene.Instantiate<Enemy>();
				case 19:
					return armoredSpiritBossScene.Instantiate<Enemy>();
				case 29:
					return multigunnerBossScene.Instantiate<Enemy>();
				case 39:
					return arcaneBossScene.Instantiate<Enemy>();
				default:
					List<PackedScene> possibilities = new()
					{
						firstBossScene,
						multigunnerBossScene,
						armoredSpiritBossScene,
						arcaneBossScene
					};

					return possibilities[Enemy.statsRng.RandiRange(0, possibilities.Count - 1)].Instantiate<Enemy>();
			}


		}

		if (gen % 10 == 9)
			return InstantiateBoss();
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
		if (gen % 10 == 9)
			return Vector2.Zero;
		return new Vector2(Enemy.statsRng.RandiRange(0, 250), MathF.Sqrt(index) * 80 * (index % 2 == 0 ? 1 : -1));
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