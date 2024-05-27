using RogueDefense.Logic.Statuses;

public partial class CorrosiveContainer : StatusContainer
{
    public override Status GetStatus()
    {
        return enemy.corrosive;
    }
}
