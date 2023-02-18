using System.Collections.Generic;
using System.Linq;
using Godot;

namespace RogueDefense.Logic.Statuses
{
    public abstract class DamagingStatus : Status
    {
        public List<DamagingStatusInstance> instances = new List<DamagingStatusInstance>();
        public override int Count => instances.Count;
        public void Add(float dpt, float duration)
        {
            if (!immune)
                instances.Add(new DamagingStatusInstance(duration, dpt));
        }
        public override void Process(float delta)
        {
            tickTimer += delta;
            if (tickTimer > TICK_INTERVAL)
            {
                tickTimer %= TICK_INTERVAL;
                Damage();
                instances = instances.Select(x => new DamagingStatusInstance(x.duration - TICK_INTERVAL, x.dpt))
                                     .Where(x => x.duration > 0)
                                     .ToList();
            }
        }
        public virtual Color DamageColor => Colors.WebGray;
        public virtual bool IgnoreArmor => false;
        public void Damage()
        {
            float damage = instances.Aggregate(0f, (x, instance) => x += instance.dpt);
            Enemy.Damage(damage, true, DamageColor, new Vector2(0, -0.6f), IgnoreArmor);
        }
    }
    public struct DamagingStatusInstance
    {
        public float duration;
        public float dpt;
        public DamagingStatusInstance(float duration, float dpt)
        {
            this.duration = duration;
            this.dpt = dpt;
        }
    }
}