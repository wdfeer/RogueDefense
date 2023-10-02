using System.Linq;
using Godot;
using RogueDefense.Logic.PlayerCore;

public partial class UpgradeScreen : Panel
{
    public static UpgradeScreen instance;
    public override void _Ready()
    {
        instance = this;
    }


    [Export]
    public PackedScene upgradeButtonScene;
    public void Activate()
    {
        Show();
        buttons = new CustomButton[]
        {
            upgradeButtonScene.Instantiate<CustomButton>(),
            upgradeButtonScene.Instantiate<CustomButton>(),
            upgradeButtonScene.Instantiate<CustomButton>()
        };

        upgrades = Upgrade.RandomUniqueUpgrades(3);
        if ((Game.Gen - 1) % 6 == 0)
        {
            upgrades[2].valueMult += 1.25f;
            buttons[2].Modulate = Colors.Red;
            upgrades[2].risky = true;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            CustomButton butt = buttons[i];

            AddChild(butt);
            int index = i;
            butt.onClick = () => OnButtonClicked(index);
            (butt.GetNode("Label") as Label).Text = upgrades[i].ToString();
        }
        buttons[0].Position += new Vector2(-250, -60);
        buttons[1].Position += new Vector2(-25, -60);
        buttons[2].Position += new Vector2(200, -60);
    }
    CustomButton[] buttons;
    Upgrade[] upgrades;
    void OnButtonClicked(int index)
    {
        if (buttons.Any(x => !IsInstanceValid(x)))
            return;
        Upgrade up = upgrades[index];
        UpgradeManager.AddUpgrade(up, Player.my.id);
        foreach (var butt in buttons)
        {
            butt.Hide();
            butt.QueueFree();
        }

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
