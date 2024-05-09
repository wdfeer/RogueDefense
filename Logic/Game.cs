using Godot;
using RogueDefense;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System;
using System.Linq;
using System.Security.Policy;

public partial class Game : Node
{
	public static Game instance;

	[Export]
	public PackedScene enemyScene;

	public override void _Ready()
	{
		SaveData.Save();

		instance = this;

		PP.currentPP = 0f;

		Enemy.enemies = new System.Collections.Generic.List<Enemy>();
		Enemy.ResetRngSeed();

		Player.my = new Player(Client.myId, SaveData.augmentAllotment);
		Client.instance.others.ForEach(x => new Player(x.id, x.augmentPoints));

		SpawnEnemiesAfterDelay();
	}
	void SpawnEnemiesAfterDelay()
		=> ToSignal(GetTree().CreateTimer(1f, false), "timeout").OnCompleted(SpawnEnemies);
	void SpawnEnemies()
	{
		for (int i = 0; i < Enemy.statsRng.RandiRange(1, 3); i++)
		{
			Enemy enemy = enemyScene.Instantiate<Enemy>();
			Enemy.enemies.Add(enemy);
			enemy.Position = new Vector2(900 + Enemy.statsRng.RandiRange(0, 250), 300 + i * 20 * (i % 2 == 0 ? 1 : -1));
			GetNode("Enemies").AddChild(enemy);
		}

		(GetNode("./LevelText") as Label).Text = $"Level {wave}";
	}
	public void OnEnemyDeath(Enemy enemy, bool netUpdate = true)
	{
		if (!IsInstanceValid(enemy))
			return;

		foreach (Player player in Player.players.Values)
		{
			foreach (PlayerHooks hook in player.hooks)
			{
				hook.OnKill(enemy);
			}
		}

		enemy.QueueFree();
		if (!NetworkManager.Singleplayer && netUpdate)
		{
			Client.instance.SendMessage(MessageType.EnemyKill, new string[1] { Enemy.enemies.FindIndex(x => x == enemy).ToString() });
		}
		if (Enemy.enemies.All(x => x.Dead))
			EndWave();
	}
	public static int Wave => instance.wave;
	private int wave = 1;
	public void EndWave()
	{
		PP.currentPP += PP.GetWavePP(wave, DefenseObjective.instance.HpRatio, Enemy.enemies.Count);
		((Label)GetNode("PPLabel")).Text = PP.currentPP.ToString("0.000") + " pp";

		Enemy.enemies = new System.Collections.Generic.List<Enemy>();
		wave++;

		SaveData.UpdateHighscore();
		SaveData.killCount++;
		SaveData.Save();

		GetTree().Paused = true;
		(GetNode("./UpgradeScreen") as UpgradeScreen).Activate();

		foreach (var keyValue in Player.players)
		{
			keyValue.Value.OnWaveEnd();
		}

		SpawnEnemiesAfterDelay();
	}


	[Export]
	PackedScene mainMenuScene;
	public void GoToMainMenu()
	{
		if (!NetworkManager.Singleplayer)
		{
			NetworkManager.NetStop();
		}
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
	}
}
