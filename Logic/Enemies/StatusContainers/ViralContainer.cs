using RogueDefense.Logic.Enemies.Statuses;

namespace RogueDefense.Logic.Enemies.StatusContainers;

public partial class ViralContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.viral;
    }
}