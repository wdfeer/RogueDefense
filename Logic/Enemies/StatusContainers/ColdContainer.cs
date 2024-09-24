using RogueDefense.Logic.Enemies.Statuses;

namespace RogueDefense.Logic.Enemies.StatusContainers;

public partial class ColdContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.cold;
    }
}