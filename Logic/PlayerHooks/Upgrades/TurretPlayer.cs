using System.Collections.Generic;
using System.Linq;
using Godot;
using RogueDefense.Logic;

namespace RogueDefense
{
    public class TurretPlayer : PlayerHooks
    {
        public int TurretCount => turrets.Count;
        public List<Node2D> turrets = new List<Node2D>();
        public void SpawnTurret()
        {
            Node2D turret = Player.turretScene.Instance() as Node2D;
            Player.AddChild(turret);
            turret.Position += new Vector2(-50f + GD.Randf() * 200f, (GD.Randf() - 0.5f) * 300);
            turrets.Add(turret);
        }
        public override void PostShoot(Bullet bullet)
        {
            foreach (var sentry in turrets)
            {
                Vector2 direction = sentry.GlobalPosition.DirectionTo(Enemy.instance.GlobalPosition).Rotated((GD.Randf() - 0.5f) * 0.05f);
                Bullet b = Player.shootManager.NewBullet(sentry.GlobalPosition, direction * ShootManager.SHOOT_SPEED);

                b.damage = bullet.damage;
                b.SetHitMultiplier(bullet.hitMult);
            }
        }
    }
}