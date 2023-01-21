using Godot;

namespace RogueDefense
{
    public abstract class PlayerHooks
    {
        public Player Player => Player.instance;
        public virtual void PreUpdate(float delta) { }
        public virtual void Update(float delta) { }
        public virtual void PostUpdate(float delta) { }


        public static PlayerHooks GetHooks<T>(Player player)
        {
            return player.hooks.Find(x => x is T);
        }
    }
}