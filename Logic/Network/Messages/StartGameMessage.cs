namespace RogueDefense.Logic.Network.Messages;

public partial class StartGameMessage : Resource, IMessage
{
    public void ClientHandle(Client client)
    {
        Lobby.Instance.GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }
}