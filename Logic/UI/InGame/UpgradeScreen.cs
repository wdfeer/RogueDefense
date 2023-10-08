using System.Linq;
using Godot;
using RogueDefense.Logic.PlayerCore;

public partial class UpgradeScreen : Panel
{
    public static UpgradeScreen instance;
    Button[] buttons;
    Upgrade[] upgrades;
    public override void _Ready()
    {
        instance = this;
        buttons = new Button[] {
            (Button)GetNode("UpgradeButton1"),
            (Button)GetNode("UpgradeButton2"),
            (Button)GetNode("UpgradeButton3"),
        };
        for (int i = 0; i < buttons.Length; i++)
        {
            Button butt = buttons[i];
            int index = i;
            butt.Pressed += () => OnButtonClicked(index);
        }
    }
    public void Activate()
    {
        Show();

        upgrades = Upgrade.RandomUniqueUpgrades(3);

        if ((Game.Gen - 1) % 6 == 0)
        {
            upgrades[2].valueMult += 1.25f;
            buttons[2].Modulate = Colors.Red;
            upgrades[2].risky = true;
        }
        else
        {
            buttons[2].Modulate = Colors.White;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            (buttons[i].GetNode("Label") as Label).Text = upgrades[i].ToString();
        }
    }

    void OnButtonClicked(int index)
    {
        if (buttons.Any(x => !IsInstanceValid(x)))
            return;
        Upgrade up = upgrades[index];
        UpgradeManager.AddUpgrade(up, Player.my.id);

        if (NetworkManager.Singleplayer)
        {
            HideAndUnpause();
        }
        else
        {
            upgradesMade++;
            Client.instance.SendMessage(MessageType.Upgrade, new string[] { Client.myId.ToString(), up.type.uniqueId.ToString(), up.Value.ToString(), up.risky ? "R" : "S" });
            if (EveryoneUpgraded())
                HideAndUnpause();
        }
    }

    public byte upgradesMade = 0; //Only for multiplayer
    public void HideAndUnpause()
    {
        Hide();
        GetTree().Paused = false;

        upgradesMade = 0;
    }
    public bool EveryoneUpgraded()
        => upgradesMade >= Client.instance.others.Count + 1;
}
