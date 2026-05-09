namespace RogueDefense.Logic.Network.Messages;

public partial class UnregisterMessage : Resource, IMessage
{
    [Export] public int id;
    
    public void ClientHandle(Client client)
    {
        client.UnregisterUser(id);
    }
}