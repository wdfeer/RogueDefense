using Godot;

namespace RogueDefense.Logic.Statuses;

public partial class Viral : SpecialStatus
{
    public override void SpecialProcess(float delta)
    {
        if (instances.Count == 0)
            return;
        enemy.dynamicDamageMult *= 1.5f;

        const int LINEAR_SCALING_BOUNDARY = 5;
        float flat;
        if (instances.Count > LINEAR_SCALING_BOUNDARY)
            flat = (Mathf.Sqrt(instances.Count - LINEAR_SCALING_BOUNDARY) + LINEAR_SCALING_BOUNDARY) / 10f;
        else
            flat = instances.Count / 10f;
        flat *= Mathf.Log(instances.Count);

        enemy.dynamicDamageMult += flat;
    }
}