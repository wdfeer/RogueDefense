namespace RogueDefense.Logic.Statuses
{
    public abstract class Status
    {
        public Enemy Enemy => Enemy.instance;
        public abstract int Count { get; }

        public bool Active => Count > 0;
        public virtual bool ShouldProcess() => Active;
        public bool immune = false;
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