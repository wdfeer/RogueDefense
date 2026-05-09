using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Network.Messages;

public partial class PositionUpdatedMessage : Resource, IMessage
{
    [Export] public int from;
    [Export] public int turretIndex;
    [Export] public Vector2 newPos;

    public void ClientHandle(Client client)
    {
        var player = PlayerManager.players[from];
        var turret = player.turrets[turretIndex];
        turret.UpdatePositionFromNetwork(newPos);
    }
}