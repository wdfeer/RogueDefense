using Godot;

namespace RogueDefense.Logic.Statuses
{
    public class Viral : SpecialStatus
    {
        public override void SpecialProcess(float delta)
        {
            Enemy.dynamicDamageMult = 1f +
                (instances.Count > 10 ? ((Mathf.Pow(instances.Count - 10, 0.7f)) + 10) : instances.Count) / 10f;
        }
    }
}