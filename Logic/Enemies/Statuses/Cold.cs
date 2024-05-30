using Godot;

namespace RogueDefense.Logic.Statuses;

public partial class Cold : SpecialStatus
{
    public override void SpecialProcess(float delta)
    {
        enemy.dynamicSpeedMult /= 1.25f + Mathf.Pow(Count, 0.33f);
    }
}