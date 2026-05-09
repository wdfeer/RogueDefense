namespace RogueDefense.Logic.Network;

public interface IMessage
{
    public void ClientHandle(Client client);
}