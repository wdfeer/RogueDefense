namespace RogueDefense.Logic.Network.Messages;

public partial class RegisterMessage : Resource, IMessage
{
    [Export] public int id;
    [Export] public string name;
    [Export] public int ability;
    [Export] public int[] augmentPoints;
    
    
    public void ClientHandle(Client client)
    {
        client.RegisterUser(this);
    }
}