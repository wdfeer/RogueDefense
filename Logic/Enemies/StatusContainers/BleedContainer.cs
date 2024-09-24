using RogueDefense.Logic.Enemies.Statuses;

namespace RogueDefense.Logic.Enemies.StatusContainers;

public partial class BleedContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.bleed;
    }
}