using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Network.Messages;

public partial class TargetSelectedMessage : Resource, IMessage
{
    [Export] public int from;
    [Export] public int enemyIndex;

    public void ClientHandle(Client client)
    {
        var player = PlayerManager.players[from];
        player.SetTarget(enemyIndex, false);
    }
}