using Godot;

namespace RogueDefense.Logic.Statuses
{
    public partial class Corrosive : SpecialStatus
    {
        public override void SpecialProcess(float delta)
        {
            if (Count == 0)
                return;
            Enemy.dynamicArmorMult /= 1.5f + (Count > 10 ? (1f + (Count - 10) / 25f) : Count / 10f);
        }
    }
}