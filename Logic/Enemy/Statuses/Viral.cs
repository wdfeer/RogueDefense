using Godot;

namespace RogueDefense.Logic.Statuses
{
    public class Viral : SpecialStatus
    {
        public override void SpecialProcess(float delta)
        {
            if (instances.Count == 0)
                return;
            Enemy.dynamicDamageMult *= 1.5f;
            Enemy.dynamicDamageMult *= 1f +
                (instances.Count > 10 ? ((Mathf.Pow(instances.Count - 10, 0.75f)) + 10) : instances.Count) / 10f;
        }
    }
}