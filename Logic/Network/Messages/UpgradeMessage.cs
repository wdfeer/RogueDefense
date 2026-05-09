using RogueDefense.Logic.Player.Core;

namespace RogueDefense.Logic.Network.Messages;

public partial class UpgradeMessage : Resource, IMessage
{
    [Export] public int from;
    [Export] public int typeIndex;
    [Export] public float value;
    [Export] public bool risky;

    public void ClientHandle(Client client)
    {
        Upgrade up = new(UpgradeType.AllTypes[typeIndex], value)
        {
            risky = risky
        };
        UpgradeManager.AddUpgrade(up, from);
        UI.InGame.UpgradeScreen.instance.upgradesMade++;
        if (UI.InGame.UpgradeScreen.instance.EveryoneUpgraded())
        {
            UI.InGame.UpgradeScreen.instance.HideAndUnpause();
        }
    }
}