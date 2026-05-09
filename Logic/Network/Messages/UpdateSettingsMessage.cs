using RogueDefense.Logic.UI.Lobby.Settings;

namespace RogueDefense.Logic.Network.Messages;

public partial class UpdateSettingsMessage : Resource, IMessage
{
    [Export] public float damage;
    [Export] public float fireRate;
    [Export] public bool healthDrain;

    public void ClientHandle(Client client)
    {
        GameSettings.totalDmgMult = damage;
        GameSettings.totalFireRateMult = fireRate;
        GameSettings.healthDrain = healthDrain;
        GameSettings.UpdateSliders();
    }
}