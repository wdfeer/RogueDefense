using Godot;

namespace RogueDefense.Logic.Statuses;

public class Viral : SpecialStatus
{
    public override void SpecialProcess(float delta)
    {
        if (Count == 0)
            return;
        enemy.dynamicDamageMult *= 1.5f;

        const int LINEAR_SCALING_BOUNDARY = 5;
        float flat;
        if (Count > LINEAR_SCALING_BOUNDARY)
            flat = (Mathf.Sqrt(Count - LINEAR_SCALING_BOUNDARY) + LINEAR_SCALING_BOUNDARY) / 10f;
        else
            flat = Count / 10f;
        flat *= Mathf.Log(Count);

        enemy.dynamicDamageMult += flat;
    }
}