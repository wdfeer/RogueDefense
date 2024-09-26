using RogueDefense.Logic.Enemy.Statuses;

namespace RogueDefense.Logic.Enemy.StatusContainers;

public partial class ViralContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.viral;
    }
}