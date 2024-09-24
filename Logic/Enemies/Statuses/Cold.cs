using Godot;

namespace RogueDefense.Logic.Enemies.Statuses;

public class Cold : SpecialStatus
{
    public override void SpecialProcess(float delta)
    {
        enemy.dynamicSpeedMult /= 1.25f + Mathf.Pow(Count, 0.33f);
    }
}