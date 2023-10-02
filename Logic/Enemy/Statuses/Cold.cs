using Godot;

namespace RogueDefense.Logic.Statuses
{
    public partial class Cold : SpecialStatus
    {
        public override void SpecialProcess(float delta)
        {
            Enemy.dynamicSpeedMult = 1f / Mathf.Pow(instances.Count + 1f, 0.33f);
        }
    }
}