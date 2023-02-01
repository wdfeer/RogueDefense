namespace RogueDefense.Logic.Statuses
{
    public abstract class Status
    {
        public Enemy Enemy => Enemy.instance;
        public abstract int GetCount();
        public bool immune = false;
        public virtual bool ShouldProcess() => GetCount() > 0;
        public void TryProcess(float delta)
        {
            if (ShouldProcess())
                Process(delta);
        }
        public abstract void Process(float delta);
        protected float tickTimer = TICK_INTERVAL;
        public const float TICK_INTERVAL = 1f;
    }
}