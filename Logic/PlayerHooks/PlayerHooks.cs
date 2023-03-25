using Godot;

namespace RogueDefense
{
    public abstract class PlayerHooks
    {
        public Player Player => Player.local;
        public virtual void PreUpdate(float delta) { }
        public virtual void PostUpgradeUpdate(float delta) { }
        public virtual void PostUpdate(float delta) { }
        public virtual void ModifyHitWithBullet(Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult) { }
        public virtual void OnHitWithBullet(Bullet b, float postCritDmg) { }
        public virtual void OnAnyHit(float afterEffectsDmg) { }
        public virtual void PreShoot(ShootManager shooter) { }
        public virtual void PostShoot(Bullet bullet) { }
        public virtual void OnKill() { }

        public static T GetLocalHooks<T>() where T : PlayerHooks
        {
            return GetHooks<T>(Player.local);
        }
        public static T GetHooks<T>(Player player) where T : PlayerHooks
        {
            return (T)player.hooks.Find(x => x is T);
        }
    }
}