using RogueDefense.Logic.Enemies.Statuses;

namespace RogueDefense.Logic.Enemies.StatusContainers;

public partial class CorrosiveContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.corrosive;
    }
}