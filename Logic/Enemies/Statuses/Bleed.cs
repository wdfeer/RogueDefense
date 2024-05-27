namespace RogueDefense.Logic.Statuses;

public partial class Bleed : DamagingStatus
{
    public override bool IgnoreArmor => true;
}