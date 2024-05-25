using Godot;
using RogueDefense.Logic;
using System;
using System.Collections.Generic;

namespace RogueDefense.Logic.PlayerCore
{
    public partial class ShootManager
    {
        readonly Player player;
        public readonly ProjectileManager projectileManager;
        public ShootManager(Player player)
        {
            this.player = player;
            projectileManager = DefenseObjective.instance.projectileManagerScene.Instantiate<ProjectileManager>();
            projectileManager.Name = $"{player.Name}'s Projectiles";
            DefenseObjective.instance.AddSibling(projectileManager);

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
            timeSinceLastShot += delta;
            if (timeSinceLastShot > shootInterval && player.target != null && GodotObject.IsInstanceValid(player.target))
            {
                timeSinceLastShot = 0;
                CreateBullets();
                shootCount++;
                PlayShootAnimation();
            }
        }
        public const float BASE_SHOOT_SPEED = 6f;
        public float shootSpeed = BASE_SHOOT_SPEED;
        public const float SPREAD_DEGREES = 10f;
        public int shootCount = 0;
        public List<Node2D> bulletSpawns = new List<Node2D>();

        public bool colored = false;
        private void CreateBullets()
        {
            player.hooks.ForEach(x => x.PreShoot(this));
            int bulletCount = MathHelper.RandomRound(multishot);
            float hitMult = 1f;
            if (bulletCount / shootInterval > 100f)
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

                    if (colored)
                    {
                        bullet.modulate = Colors.LightCyan;
                    }
                }
            }

            colored = false;
        }

        public Bullet Shoot(Vector2 pos, float speed, float spreadDeg = SPREAD_DEGREES)
        {
            Bullet bullet = projectileManager.SpawnBullet(pos);
            bullet.owner = player;

            Vector2 velocity = speed * pos.DirectionTo(player.target.GlobalPosition);
            bullet.velocity = velocity.Rotated(Mathf.DegToRad(GD.Randf() * spreadDeg - spreadDeg / 2f));

            return bullet;
        }

        public void ClearBullets(Func<Projectile, bool> exception = null)
        {
            if (exception != null)
                projectileManager.ClearProjectiles(exception);
            else
                projectileManager.ClearProjectiles();
        }

        private void PlayShootAnimation()
        {
            float speed = shootInterval < 0.5f ? Mathf.Pow(0.5f / shootInterval, 2) : 1f;
            foreach (Turret turret in player.turrets)
            {
                turret.animationPlayer.Stop();
                turret.animationPlayer.Play("ShootEffects", customSpeed: speed);
            }
        }
    }
}
