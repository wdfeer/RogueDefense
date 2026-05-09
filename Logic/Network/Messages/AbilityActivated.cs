using RogueDefense.Logic.Player.Core;
using RogueDefense.Logic.Player.Hooks;

namespace RogueDefense.Logic.Network.Messages;

public partial class AbilityActivatedMessage : Resource, IMessage
{
    [Export] public int from;
    [Export] public int abilityTypeIndex;


    public void ClientHandle(Client client)
    {
        string username = client.GetUserData(from).name;
        ActiveAbility ability = (ActiveAbility)PlayerManager.players[from].hooks
            .Find(x => x.GetType() == AbilityManager.abilityTypes[abilityTypeIndex]);
        ability.ActivateTryShare();
        UI.InGame.NotificationPopup.Notify($"{username} used {ability.GetName()}", 1.5f);
    }
}