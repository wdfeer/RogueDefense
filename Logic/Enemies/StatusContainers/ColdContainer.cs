using RogueDefense.Logic.Statuses;

public partial class ColdContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.cold;
    }
}
