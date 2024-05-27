using Godot;

namespace RogueDefense.Logic.Statuses;

public partial class Viral : SpecialStatus
{
    public override void SpecialProcess(float delta)
    {
        if (instances.Count == 0)
            return;
        enemy.dynamicDamageMult *= 1.5f;
        enemy.dynamicDamageMult *= 1f +
            (instances.Count > 10 ? (Mathf.Pow(instances.Count - 10, 0.75f) + 10) : instances.Count) / 10f;
    }
}