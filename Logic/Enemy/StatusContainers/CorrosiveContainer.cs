using RogueDefense.Logic.Enemy.Statuses;

namespace RogueDefense.Logic.Enemy.StatusContainers;

public partial class CorrosiveContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.corrosive;
    }
}