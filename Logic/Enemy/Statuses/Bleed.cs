namespace RogueDefense.Logic.Enemy.Statuses;

public class Bleed : DamagingStatus
{
    public override bool IgnoreArmor => true;
}