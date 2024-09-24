using System.Linq;
using Godot;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic;

public partial class Game : Node
{
	public static Game instance;
	public override void _Ready()
	{
		instance = this;

		SaveData.Save();

		PP.currentPP = 0f;

		Enemy.enemies = new System.Collections.Generic.List<Enemy>();
		Enemy.ResetRngSeed();

		PlayerManager.my = new Player(Client.myId, SaveData.augmentAllotment);
		Client.instance.others.ForEach(x => new Player(x.id, x.augmentPoints));

		SpawnEnemiesAfterDelay();
	}
	void SpawnEnemiesAfterDelay()
		=> ToSignal(GetTree().CreateTimer(0.5, false), "timeout").OnCompleted(() =>
		{
			((EnemySpawner)GetNode("EnemySpawner")).SpawnEnemies();
			background.UpdateBackground(GetStage());
		});
	public void OnEnemyDeath(Enemy enemy, bool netUpdate = true)
	{
		if (!IsInstanceValid(enemy))
			return;

		foreach (Player player in PlayerManager.players.Values)
		{
			foreach (PlayerHooks.PlayerHooks hook in player.hooks)
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
	private int wave = 0;
	public static int GetStage()
		=> Wave / 10 + 1;
	[Export]
	private Background background;
	public void EndWave()
	{
		PP.currentPP += PP.GetWavePP(wave, DefenseObjective.instance.HpRatio, Enemy.enemies.Count);
		((Label)GetNode("PPLabel")).Text = PP.currentPP.ToString("0.000") + " pp";

		Enemy.enemies = new System.Collections.Generic.List<Enemy>();
		wave++;

		GetNode<UI.InGame.UpgradeScreen>("./UpgradeScreen").ResetNotificationLabel();

		SaveData.UpdateHighscore();
		SaveData.killCount++;
		SaveData.Save();

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