using RogueDefense.Logic;
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
        public virtual void ModifyHitEnemyWithProj(Enemy enemy, Projectile p, ref float damagePreCrit, ref int critLevel, ref float critMult) { }
        public virtual void OnHitWithProj(Enemy enemy, Projectile p, float postCritDmg) { }
        public virtual void OnAnyHit(float afterEffectsDmg) { }
        public virtual void PreShoot(ShootManager shooter) { }
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