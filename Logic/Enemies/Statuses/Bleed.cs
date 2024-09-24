namespace RogueDefense.Logic.Enemies.Statuses;

public class Bleed : DamagingStatus
{
    public override bool IgnoreArmor => true;
}