using System.Diagnostics;

namespace RogueDefense.Logic.Network.Messages;

public partial class EnemyKillMessage : Resource, IMessage
{
    [Export] public int index;

    public void ClientHandle(Client client)
    {
        Debug.Assert(IsInstanceValid(Game.instance));

        if (index >= Enemy.Enemy.enemies.Count || Enemy.Enemy.enemies[index] == null)
            return;
        Enemy.Enemy.enemies[index].Die(false);
    }
}