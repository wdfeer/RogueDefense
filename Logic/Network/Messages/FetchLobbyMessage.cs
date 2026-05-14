namespace RogueDefense.Logic.Network.Messages;

public partial class FetchLobbyMessage : Resource, IMessage
{
    [Export] public int id;
    [Export] public RegisterMessage[] others;

    public void ClientHandle(Client client)
    {
        Client.ChangeSceneToLobby();
        GD.Print($"Sending Register Message with augments {string.Join(",", Save.UserData.augmentAllotment)}");
        Client.RegisterSelf();
        Client.myId = id;
        foreach (var user in others)
        {
            client.RegisterUser(user);
        }
    }
}