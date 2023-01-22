using Godot;
using RogueDefense;
using System;
using System.Security.Policy;

public class Game : Node2D
{
    public static Game instance;

    [Export]
    public PackedScene enemyScene;

    public Player myPlayer;
    public Enemy enemy;
    public override void _Ready()
    {
        instance = this;
        myPlayer = GetNode("./MyPlayer") as Player;
    }

    public override void _Process(float delta)
    {
        TimerManager.Process(delta);
        if (enemy == null)
        {
            enemy = enemyScene.Instance() as Enemy;
            enemy.Position = new Vector2(900, 300);
            AddChild(enemy);
        }
    }

    public int generation = 0;
    public void DeleteEnemy()
    {
        enemy.QueueFree();
        generation++;
        enemy = null;

        GetTree().Paused = true;
        (GetNode("./UpgradeScreen") as UpgradeScreen).Activate();
        (GetNode("./UpgradeScreen/LevelText") as Label).Text = $"Level {generation}";

        myPlayer.shootManager.ClearBullets();
        myPlayer.hpManager.Hp = myPlayer.hpManager.maxHp;
    }
}
