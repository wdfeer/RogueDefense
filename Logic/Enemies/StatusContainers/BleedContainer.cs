using RogueDefense.Logic.Statuses;

public partial class BleedContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.bleed;
    }
}