using Godot;
using RogueDefense;

public class UpgradeScreen : Panel
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
            upgradeButtonScene.Instance() as CustomButton,
            upgradeButtonScene.Instance() as CustomButton,
            upgradeButtonScene.Instance() as CustomButton
        };
        bool basedUpgrades = Game.instance.generation % 5 == 0;
        upgrades = new Upgrade[]
        {
            Upgrade.RandomUpgrade(basedUpgrades),
            Upgrade.RandomUpgrade(basedUpgrades),
            Upgrade.RandomUpgrade(basedUpgrades),
        };
        for (int i = 0; i < buttons.Length; i++)
        {
            CustomButton butt = buttons[i];

            AddChild(butt);
            int index = i;
            butt.onClick = () => OnButtonClicked(index);
            butt.Text = upgrades[i].ToString();
        }
        buttons[0].RectPosition += new Vector2(-240, -40);
        buttons[1].RectPosition += new Vector2(0, -40);
        buttons[2].RectPosition += new Vector2(240, -40);
    }
    CustomButton[] buttons;
    Upgrade[] upgrades;
    void OnButtonClicked(int index)
    {
        Upgrade up = upgrades[index];
        Player.localInstance.upgradeManager.AddUpgrade(up);
        foreach (var butt in buttons)
        {
            butt.QueueFree();
        }

        if (NetworkManager.Singleplayer)
        {
            HideAndUnpause();
        }
        else
        {
            upgradesMade++;
            Client.instance.SendMessage(MessageType.Upgrade, new string[] { ((int)up.type).ToString(), up.value.ToString() });
            if (AllUpgradesMadeInMP())
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
    public bool AllUpgradesMadeInMP()
        => upgradesMade >= Client.instance.others.Count + 1;
}
