using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic.Enemies;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.PlayerCore;

public partial class Player
{
    private void UpdateMovement(double delta)
    {
        Vector2 inputDirection = Vector2.Zero;
        if (!controlledTurret.Stunned)
            inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        foreach (Turret turret in turrets)
        {
            turret.Velocity /= 2;
            if (turret == controlledTurret)
            {
                turret.Velocity += inputDirection * Turret.SPEED * 650 * (float)delta;
            }

            turret.MoveAndSlide();
        }

        if (NetworkManager.Singleplayer || inputDirection == Vector2.Zero)
            return;
        Vector2 pos = controlledTurret.GlobalPosition;
        SendPositionUpdateMessage(Client.myId, turrets.FindIndex(x => x == controlledTurret), pos.X, pos.Y);
    }

    private static void SendPositionUpdateMessage(int client, int turretIndex, float x, float y)
    {
        Client.instance.SendMessage(MessageType.PositionUpdated,
            new string[] { client.ToString(), turretIndex.ToString(), x.ToString(), y.ToString() });
    }

    public List<Turret> turrets = new List<Turret>();
    public IEnumerable<Turret> ActiveTurrets => turrets.Where(x => !x.Stunned);
    public Turret controlledTurret;
    public Enemy target;

    private bool IsValidTarget(Enemy enemy)
        => enemy != null && GodotObject.IsInstanceValid(enemy) && enemy.Targetable;

    private void FindTarget()
    {
        for (int i = 0; i < Enemy.enemies.Count; i++)
        {
            Enemy enemy = Enemy.enemies[i];
            if (IsValidTarget(enemy))
            {
                SetTarget(i);
                return;
            }
        }
    }

    public void SetTarget(int enemyIndex, bool netUpdate = true)
    {
        if (enemyIndex >= Enemy.enemies.Count)
            return;

        Enemy enemy = Enemy.enemies[enemyIndex];

        if (!IsValidTarget(enemy))
            return;

        target = enemy;
        foreach (Turret turret in turrets)
        {
            turret.target = enemy;
        }

        if (netUpdate && Client.client != null)
        {
            Client.instance.SendMessage(MessageType.TargetSelected,
                new[] { Client.myId.ToString(), enemyIndex.ToString() });
        }
    }

    public void SpawnTurret()
    {
        controlledTurret = DefenseObjective.instance.turretScene.Instantiate<Turret>();
        controlledTurret.owner = this;
        DefenseObjective.instance.AddChild(controlledTurret);
        controlledTurret.Position += new Vector2(-50f + GD.Randf() * 200f, (GD.Randf() - 0.5f) * 300);
        turrets.Add(controlledTurret);

        controlledTurret.SetLabel(string.Concat(Name.Take(3)).ToUpper());

        controlledTurret.target = target;
    }
}