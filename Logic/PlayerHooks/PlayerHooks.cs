using System;
using Godot;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense
{
    public abstract class PlayerHooks
    {
        public PlayerHooks(Player player)
        {
            this.player = player;
        }
        public Player player;
        public virtual void PreUpdate(float delta) { }
        public virtual void PostUpgradeUpdate(float delta) { }
        public virtual void PostUpdate(float delta) { }
        [Obsolete("Use ModifyHitEnemyWithBullet instead")]
        public virtual void ModifyHitWithBullet(Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult) { }
        public virtual void ModifyHitEnemyWithBullet(Enemy enemy, Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult) { }
        public virtual void OnHitWithBullet(Enemy enemy, Bullet b, float postCritDmg) { }
        public virtual void OnAnyHit(float afterEffectsDmg) { }
        public virtual void PreShoot(ShootManager shooter) { }
        public virtual void PostShoot(Bullet bullet) { }
        public virtual void OnWaveEnd() { }
        public virtual void OnKill(Enemy enemy) { }

        public static T GetLocalHooks<T>() where T : PlayerHooks
        {
            return GetHooks<T>(Player.my);
        }
        public static T GetHooks<T>(Player player) where T : PlayerHooks
        {
            return (T)player.hooks.Find(x => x is T);
        }
    }
}