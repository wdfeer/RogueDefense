using RogueDefense.Logic.Statuses;

public partial class ViralContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.viral;
    }
}
