using RogueDefense.Logic.Enemy.Statuses;

namespace RogueDefense.Logic.Enemy.StatusContainers;

public partial class ColdContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.cold;
    }
}