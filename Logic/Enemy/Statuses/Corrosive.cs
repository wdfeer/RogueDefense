using Godot;

namespace RogueDefense.Logic.Statuses
{
    public class Corrosive : SpecialStatus
    {
        public override void SpecialProcess(float delta)
        {
            Enemy.dynamicArmorMult /= 1f + (Count() > 10 ? (1f + (Count() - 10) / 25f) : Count() / 10f);
        }
    }
}