using RogueDefense.Logic.Enemy.Statuses;

namespace RogueDefense.Logic.Enemy.StatusContainers;

public partial class BleedContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.bleed;
    }
}