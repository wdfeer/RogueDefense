using Godot;
using RogueDefense.Logic;
using System;
using System.Collections.Generic;

namespace RogueDefense
{
    public class ShootManager
    {
        readonly Player player;
        public ShootManager(Player player)
        {
            this.player = player;
        }
        public float baseDamage = 1;
        public float damage = 1;
        public float baseShootInterval = 1;
        public float shootInterval = 1;
        float timeSinceLastShot = 0;
        public float baseMultishot = 1f;
        public float multishot = 1f;
        public void Process(float delta)
        {
            timeSinceLastShot += delta;
            if (timeSinceLastShot > shootInterval)
            {
                timeSinceLastShot = 0;
                CreateBullets();
            }
        }
        public List<Bullet> bullets = new List<Bullet>();
        public const float SPREAD_DEGREES = 16f;
        private void CreateBullets()
        {
            player.hooks.ForEach(x => x.PreShoot(this));
            int bulletCount = MathHelper.RandomRound(multishot);
            float hitMult = 1f;
            if (bulletCount / shootInterval > 10f)
            {
                hitMult = bulletCount / 3f;
                bulletCount = 3;
            }
            for (int i = 0; i < bulletCount * 1; i++)
            {
                Bullet bullet = Shoot(6f);
                bullet.damage = damage;
                bullet.SetHitMultiplier(MathHelper.RandomRound(hitMult));

                player.hooks.ForEach(x => x.PostShoot(bullet));
            }
        }
        public Bullet Shoot(float speed, float spread = -1)
        {
            Bullet bullet = player.bulletScene.Instance() as Bullet;
            bullet.velocity = new Godot.Vector2(1f * speed, 0).Rotated(spread == -1 ? Mathf.Deg2Rad(GD.Randf() * SPREAD_DEGREES - SPREAD_DEGREES / 2f) : spread);
            bullet.Position = player.Position + new Godot.Vector2(20, 0);
            Game.instance.AddChild(bullet);
            bullets.Add(bullet);
            return bullet;
        }
        public void ClearBullets(Func<Bullet, bool> filter = null)
        {
            foreach (Bullet bull in bullets)
            {
                if (Godot.Object.IsInstanceValid(bull))
                {
                    if (filter != null && !filter(bull))
                        continue;
                    bull.QueueFree();
                }
            }
            bullets = new List<Bullet>();
        }
    }
}
