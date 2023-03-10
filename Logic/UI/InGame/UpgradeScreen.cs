using System.Linq;
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
        upgrades = Upgrade.RandomUniqueUpgrades(3);
        for (int i = 0; i < buttons.Length; i++)
        {
            CustomButton butt = buttons[i];

            AddChild(butt);
            int index = i;
            butt.onClick = () => OnButtonClicked(index);
            (butt.GetNode("Label") as Label).Text = upgrades[i].ToString();
        }
        buttons[0].RectPosition += new Vector2(-250, -60);
        buttons[1].RectPosition += new Vector2(-25, -60);
        buttons[2].RectPosition += new Vector2(200, -60);
    }
    CustomButton[] buttons;
    Upgrade[] upgrades;
    void OnButtonClicked(int index)
    {
        if (buttons.Any(x => !IsInstanceValid(x)))
            return;
        Upgrade up = upgrades[index];
        Player.localInstance.upgradeManager.AddUpgrade(up);
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
            Client.instance.SendMessage(MessageType.Upgrade, new string[] { up.type.uniqueId.ToString(), up.value.ToString() });
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
