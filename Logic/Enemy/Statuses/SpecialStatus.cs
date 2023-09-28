using System.Collections.Generic;
using System.Linq;

namespace RogueDefense.Logic.Statuses
{
    public abstract class SpecialStatus : Status
    {
        public List<float> instances = new List<float>();
        public override int Count => instances.Count;
        public void Add(float duration)
        {
            if (!immune)
                instances.Add(duration);
        }
        public override void Process(float delta)
        {
            tickTimer += (float)delta;
            if (tickTimer > TICK_INTERVAL)
            {
                tickTimer %= TICK_INTERVAL;
                instances = instances.Select(x => x - TICK_INTERVAL)
                                     .Where(x => x > 0)
                                     .ToList();
            }
            SpecialProcess(delta);
        }
        public virtual void SpecialProcess(float delta) { }
    }
}