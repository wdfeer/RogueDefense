using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class TurretPlayer : PlayerHooks
    {
        public TurretPlayer()
        {
            SpawnTurret();
            if (!NetworkManager.Singleplayer)
            {
                for (int i = 0; i < Client.instance.others.Count; i++)
                {
                    SpawnTurret();
                }
            }
        }
        public int TurretCount => turrets.Count;
        public List<Node2D> turrets = new List<Node2D>();
        public void SpawnTurret()
        {
            Node2D turret = Player.turretScene.Instance() as Node2D;
            Player.AddChild(turret);
            turret.Position += new Vector2(-50f + GD.Randf() * 200f, (GD.Randf() - 0.5f) * 300);
            turrets.Add(turret);

            Player.shootManager.bulletSpawns.Add(turret.GlobalPosition);
        }
    }
}