using System.Linq;
using Godot;
using RogueDefense.Logic.Enemy;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Hooks;
using RogueDefense.Logic.Save;
using EnemySpawner = RogueDefense.Logic.Enemy.EnemySpawner;
using UserData = RogueDefense.Logic.Save.UserData;

namespace RogueDefense.Logic;

public partial class Game : Node
{
	public static Game instance;
	public override void _Ready()
	{
		instance = this;

		SaveManager.Save();

		PP.currentPP = 0f;

		Enemy.Enemy.enemies = new System.Collections.Generic.List<Enemy.Enemy>();
		Enemy.Enemy.ResetRngSeed();

		PlayerManager.my = new Player.Core.Player(Client.myId, UserData.augmentAllotment);
		Client.instance.others.ForEach(x => new Player.Core.Player(x.id, x.augmentPoints));

		SpawnEnemiesAfterDelay();
	}
	void SpawnEnemiesAfterDelay()
		=> ToSignal(GetTree().CreateTimer(0.5, false), "timeout").OnCompleted(() =>
		{
			((EnemySpawner)GetNode("EnemySpawner")).SpawnEnemies();
			background.UpdateBackground(GetStage());
		});
	public void OnEnemyDeath(Enemy.Enemy enemy, bool netUpdate = true)
	{
		if (!IsInstanceValid(enemy))
			return;

		foreach (Player.Core.Player player in PlayerManager.players.Values)
		{
			foreach (PlayerHooks hook in player.hooks)
			{
				hook.OnKill(enemy);
			}
		}

		enemy.QueueFree();
		if (!NetworkManager.Singleplayer && netUpdate)
		{
			Client.instance.SendMessage(MessageType.EnemyKill, new string[1] { Enemy.Enemy.enemies.FindIndex(x => x == enemy).ToString() });
		}
		if (Enemy.Enemy.enemies.All(x => x.Dead))
			EndWave();
	}
	public static int Wave => instance.wave;
	private int wave = 0;
	public static int GetStage()
		=> Wave / 10 + 1;
	[Export]
	private UI.Background background;
	public void EndWave()
	{
		PP.currentPP += PP.GetWavePP(wave, DefenseObjective.instance.HpRatio, Enemy.Enemy.enemies.Count);
		((Label)GetNode("PPLabel")).Text = PP.currentPP.ToString("0.000") + " pp";

		Enemy.Enemy.enemies = new System.Collections.Generic.List<Enemy.Enemy>();
		wave++;

		GetNode<UI.InGame.UpgradeScreen>("./UpgradeScreen").ResetNotificationLabel();

		SaveManager.UpdateHighscore();
		UserData.killCount++;
		SaveManager.Save();

		GetTree().Paused = true;
		GetNode<UI.InGame.UpgradeScreen>("./UpgradeScreen").Activate();

		foreach (var keyValue in PlayerManager.players)
		{
			keyValue.Value.OnWaveEnd();
		}

		SpawnEnemiesAfterDelay();
		(GetNode("./LevelText") as Label).Text = $"Stage {GetStage()} - {wave % 10 + 1}";
	}

	public void GoToMainMenu()
	{
		if (!NetworkManager.Singleplayer)
		{
			NetworkManager.NetStop();
		}
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
	}
}