using Godot;

namespace RogueDefense
{
    public abstract class PlayerHooks
    {
        public Player Player => Player.localInstance;
        public virtual void PreUpdate(float delta) { }
        public virtual void Update(float delta) { }
        public virtual void PostUpgradeUpdate(float delta) { }
        public virtual void PostUpdate(float delta) { }
        public virtual void ModifyHitWithBullet(Bullet b, ref float damagePreCrit, ref int critLevel, ref float critMult) { }

        public static PlayerHooks GetHooks<T>(Player player)
        {
            return player.hooks.Find(x => x is T);
        }
    }
}