namespace RogueDefense.Logic.Statuses
{
    public class Bleed : DamagingStatus
    {
        public override bool IgnoreArmor => true;
    }
}