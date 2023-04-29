using Godot;
using RogueDefense;
using System;
using System.Security.Policy;

public class Game : Node2D
{
    public static Game instance;

    [Export]
    public PackedScene enemyScene;

    public Enemy enemy;
    public override void _Ready()
    {
        RogueDefense.UserSaveData.Save();

        instance = this;

        Enemy.ResetRngSeed();

        Player.my = new Player(Client.myId);
        Client.instance.others.ForEach(x => new Player(x.id));
    }

    public override void _Process(float delta)
    {
        if (enemy == null)
        {
            SpawnEnemy();
        }
    }
    void SpawnEnemy()
    {
        enemy = enemyScene.Instance() as Enemy;
        enemy.Position = new Vector2(900, 300);
        AddChild(enemy);
    }

    public int generation = 1;
    public void DeleteEnemy(bool netUpdate)
    {
        if (enemy == null)
            return;

        if (!NetworkManager.Singleplayer && netUpdate)
        {
            Client.instance.SendMessage(MessageType.EnemyKill, new string[0]);
        }

        enemy.QueueFree();
        enemy = null;
        generation++;

        UserSaveData.UpdateHighscore();
        UserSaveData.killCount++;
        UserSaveData.Save();

        GetTree().Paused = true;
        (GetNode("./UpgradeScreen") as UpgradeScreen).Activate();
        (GetNode("./UpgradeScreen/LevelText") as Label).Text = $"Level {generation}";

        foreach (var item in Player.players)
        {
            item.Value.shootManager.ClearBullets();
            item.Value.hooks.ForEach(x => x.OnKill());
            item.Value.upgradeManager.UpdateUpgrades();
            item.Value.upgradeManager.UpdateUpgradeText();
            item.Value.shootManager.shootCount = 0;
        }
    }
}
