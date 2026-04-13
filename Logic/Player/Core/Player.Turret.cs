using System.Collections.Generic;
using System.Linq;
using RogueDefense.Logic.Network;

namespace RogueDefense.Logic.Player.Core;

public partial class Player
{
    public const double POSITION_UPDATE_INTERVAL = 0.1;
    private double positionUpdateTimer;
    private Vector2? lastSentPosition;

    private void UpdateMovement(double delta)
    {
        Vector2 inputDirection = Vector2.Zero;
        if (!controlledTurret.Stunned)
            inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        foreach (Turret.Turret turret in turrets)
        {
            turret.Velocity /= 2;
            if (turret == controlledTurret)
            {
                turret.Velocity += inputDirection * Turret.Turret.SPEED * 650 * (float)delta;
            }

            turret.MoveAndSlide();
        }

        // Send position update over the network
        Vector2 pos = controlledTurret.GlobalPosition;
        if (!NetworkManager.Singleplayer && (inputDirection != Vector2.Zero || pos != lastSentPosition))
        {
            positionUpdateTimer += delta;
            if (positionUpdateTimer > POSITION_UPDATE_INTERVAL)
            {
                SendPositionUpdateMessage(Client.myId, turrets.FindIndex(x => x == controlledTurret), pos.X, pos.Y);

                lastSentPosition = pos;
                positionUpdateTimer = 0.0;
            }
        }
    }

    private static void SendPositionUpdateMessage(int client, int turretIndex, float x, float y)
    {
        Client.instance.SendMessage(MessageType.PositionUpdated,
            [client.ToString(), turretIndex.ToString(), x.ToString(), y.ToString()]);
    }

    public List<Turret.Turret> turrets = [];
    public IEnumerable<Turret.Turret> ActiveTurrets => turrets.Where(x => !x.Stunned);
    public Turret.Turret controlledTurret;
    public Enemy.Enemy target;

    private bool IsValidTarget(Enemy.Enemy enemy)
        => enemy != null && GodotObject.IsInstanceValid(enemy) && enemy.Targetable;

    private void FindTarget()
    {
        for (int i = 0; i < Enemy.Enemy.enemies.Count; i++)
        {
            Enemy.Enemy enemy = Enemy.Enemy.enemies[i];
            if (IsValidTarget(enemy))
            {
                SetTarget(i);
                return;
            }
        }
    }

    public void SetTarget(int enemyIndex, bool netUpdate = true)
    {
        if (enemyIndex >= Enemy.Enemy.enemies.Count)
            return;

        Enemy.Enemy enemy = Enemy.Enemy.enemies[enemyIndex];

        if (!IsValidTarget(enemy))
            return;

        target = enemy;
        foreach (Turret.Turret turret in turrets)
        {
            turret.target = enemy;
        }

        if (netUpdate && Client.client != null)
        {
            Client.instance.SendMessage(MessageType.TargetSelected,
                [Client.myId.ToString(), enemyIndex.ToString()]);
        }
    }

    public void SpawnTurret()
    {
        controlledTurret = DefenseObjective.instance.turretScene.Instantiate<Turret.Turret>();
        controlledTurret.owner = this;
        DefenseObjective.instance.AddChild(controlledTurret);
        controlledTurret.Position += new Vector2(-50f + GD.Randf() * 200f, (GD.Randf() - 0.5f) * 300);
        turrets.Add(controlledTurret);

        controlledTurret.SetLabel(string.Concat(Name.Take(3)).ToUpper());

        controlledTurret.target = target;
    }
}