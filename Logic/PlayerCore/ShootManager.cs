using Godot;
using RogueDefense.Logic;
using System;
using System.Collections.Generic;

namespace RogueDefense.Logic.PlayerCore
{
    public partial class ShootManager
    {
        readonly Player player;
        public ShootManager(Player player)
        {
            this.player = player;
            baseDamage = 1f + player.augmentPoints[0] * AugmentContainer.STAT_PER_POINT[0];
            baseShootInterval = 1f / (1f + player.augmentPoints[1] * AugmentContainer.STAT_PER_POINT[1]);
            baseMultishot = 1f + player.augmentPoints[2] * AugmentContainer.STAT_PER_POINT[2];
        }
        public float baseDamage;
        public float damage = 1;
        public float baseShootInterval = 1;
        public float shootInterval = 1;
        float timeSinceLastShot = 0;
        public float baseMultishot = 1f;
        public float multishot = 1f;
        public void Process(float delta)
        {
            timeSinceLastShot += (float)delta;
            if (timeSinceLastShot > shootInterval)
            {
                timeSinceLastShot = 0;
                CreateBullets();
                shootCount++;
                PlayShootAnimation();
            }
        }
        public List<Bullet> bullets = new List<Bullet>();
        public const float BASE_SHOOT_SPEED = 6f;
        public float shootSpeed = BASE_SHOOT_SPEED;
        public const float SPREAD_DEGREES = 10f;
        public int shootCount = 0;
        public List<Node2D> bulletSpawns = new List<Node2D>();
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
            for (int i = 0; i < bulletSpawns.Count; i++)
            {
                for (int k = 0; k < bulletCount * 1; k++)
                {
                    Bullet bullet = Shoot(bulletSpawns[i].GlobalPosition, shootSpeed);
                    bullet.damage = damage;
                    bullet.SetHitMultiplier(MathHelper.RandomRound(hitMult));

                    player.hooks.ForEach(x => x.PostShoot(bullet));
                }
            }

        }
        public Bullet Shoot(Vector2 pos, float speed, float spread = -1)
        {
            Vector2 velocity = speed * pos.DirectionTo(Game.instance.enemy.GlobalPosition);
            Bullet bullet = NewBullet(pos, velocity.Rotated(spread == -1 ? Mathf.DegToRad(GD.Randf() * SPREAD_DEGREES - SPREAD_DEGREES / 2f) : spread));
            return bullet;
        }
        public Bullet NewBullet(Vector2 gposition, Vector2 velocity)
        {
            Bullet bullet = DefenseObjective.instance.bulletScene.Instantiate<Bullet>();
            bullet.owner = player;
            bullet.velocity = velocity;
            bullet.GlobalPosition = gposition;
            Game.instance.AddChild(bullet);
            bullets.Add(bullet);
            return bullet;
        }
        public void ClearBullets(Func<Bullet, bool> filter = null)
        {
            foreach (Bullet bull in bullets)
            {
                if (Godot.GodotObject.IsInstanceValid(bull))
                {
                    if (filter != null && !filter(bull))
                        continue;
                    bull.QueueFree();
                }
            }
            bullets = new List<Bullet>();
        }

        private void PlayShootAnimation()
        {
            float speed = shootInterval < 0.5f ? Mathf.Pow(0.5f / shootInterval, 2) : 1f;
            foreach (Turret turret in player.turrets)
            {
                turret.animationPlayer.Stop();
                turret.animationPlayer.Play("ShootEffects");
            }
        }
    }
}
