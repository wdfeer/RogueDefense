namespace RogueDefense.Logic.Network.Messages;

public partial class DeathMessage : Resource, IMessage
{
    public void ClientHandle(Client client)
    {
        DefenseObjective.instance.Death(false);
    }
}