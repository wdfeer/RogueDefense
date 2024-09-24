using System;
using System.Linq;
using Godot;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.PlayerCore;

namespace RogueDefense.Logic.UI.InGame;

public partial class UpgradeScreen : Panel
{
    public static UpgradeScreen instance;
    Button[] buttons;
    Upgrade[] upgrades;
    public override void _Ready()
    {
        instance = this;
        buttons = new Button[] {
            GetNode<Button>("UpgradeButton1"),
            GetNode<Button>("UpgradeButton2"),
            GetNode<Button>("UpgradeButton3"),
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
        SetButtonsVisibility(true);
        Show();

        upgrades = Upgrade.RandomUniqueUpgrades(3);

        if (Game.Wave % 5 == 0)
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
    public void ResetNotificationLabel()
    {
        var label = GetNode<Label>("NotificationLabel");

        int highscore = Math.Max(SaveData.highscoreSingleplayer, SaveData.highscoreMultiplayer);
        if (Game.Wave > highscore && Game.Wave % 10 == 0)
        {
            label.Visible = true;
            int stage = Game.Wave / 10;
            label.Text = $"Stage {stage} Clear:\n+{stage} Augment Point{(stage == 1 ? "" : "s")}";
        }
        else label.Visible = false;
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
            Network.Client.instance.SendMessage(MessageType.Upgrade, new string[] { Network.Client.myId.ToString(), up.type.uniqueId.ToString(), up.Value.ToString(), up.risky ? "R" : "S" });
            if (EveryoneUpgraded())
                HideAndUnpause();
            else SetButtonsVisibility(false);
        }
    }

    public byte upgradesMade = 0; //Only for multiplayer
    void SetButtonsVisibility(bool show)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Visible = show;
        }
    }
    public void HideAndUnpause()
    {
        Hide();
        GetTree().Paused = false;

        upgradesMade = 0;
    }
    public bool EveryoneUpgraded()
        => upgradesMade >= NetworkManager.PlayerCount;
}