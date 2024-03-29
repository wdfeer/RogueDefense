using Godot;
using RogueDefense;
using RogueDefense.Logic;
using RogueDefense.Logic.PlayerCore;
using System;
using System.Security.Policy;

public partial class Game : Node2D
{
	public static Game instance;

	[Export]
	public PackedScene enemyScene;

	public Enemy enemy;
	public override void _Ready()
	{
		RogueDefense.SaveData.Save();

		instance = this;

		PP.currentPP = 0f;

		Enemy.ResetRngSeed();

		Player.my = new Player(Client.myId, SaveData.augmentAllotment);
		Client.instance.others.ForEach(x => new Player(x.id, x.augmentPoints));
	}

	public override void _Process(double delta)
	{
		if (enemy == null)
		{
			SpawnEnemy();
		}
	}
	void SpawnEnemy()
	{
		enemy = enemyScene.Instantiate<Enemy>();
		enemy.Position = new Vector2(900, 300);
		AddChild(enemy);

		(GetNode("./LevelText") as Label).Text = $"Level {wave}";
	}

	public static int Wave => instance.wave;
	private int wave = 1;
	public void OnWaveEnd(bool netUpdate)
	{
		if (enemy == null)
			return;

		if (!NetworkManager.Singleplayer && netUpdate)
		{
			Client.instance.SendMessage(MessageType.EnemyKill, new string[0]);
		}

		PP.currentPP += PP.GetKillPP(wave, DefenseObjective.instance.HpRatio);
		((Label)GetNode("PPLabel")).Text = PP.currentPP.ToString("0.000") + " pp";

		enemy.QueueFree();
		enemy = null;
		wave++;

		SaveData.UpdateHighscore();
		SaveData.killCount++;
		SaveData.Save();

		GetTree().Paused = true;
		(GetNode("./UpgradeScreen") as UpgradeScreen).Activate();

		foreach (var keyValue in Player.players)
		{
			keyValue.Value.OnEnemyKill();
		}
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
