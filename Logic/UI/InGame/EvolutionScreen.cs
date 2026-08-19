using System;
using System.Linq;
using RogueDefense.Logic.Network;
using RogueDefense.Logic.Player.Core;
using UserData = RogueDefense.Logic.Save.UserData;

namespace RogueDefense.Logic.UI.InGame;

public partial class EvolutionScreen : Panel
{
    public static EvolutionScreen instance;
    Button[] buttons;
    Evolution[] evolutions;

    public override void _Ready()
    {
        instance = this;
        buttons =
        [
            GetNode<Button>("Button1"),
            GetNode<Button>("Button2"),
            GetNode<Button>("Button3")
        ];
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

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetNode<Label>("Label").Text = evolutions[i].ToString();
        }
    }

    public void ResetNotificationLabel()
    {
        var label = GetNode<Label>("NotificationLabel");

        int highscore = Math.Max(UserData.highscoreSingleplayer, UserData.highscoreMultiplayer);
        if (Game.Wave > highscore && Game.Wave % 10 == 0)
        {
            label.Visible = true;
            int stage = Game.Wave / 10;
            label.Text = $"Stage {stage} Clear:\n+1 Augment Point";
        }
        else label.Visible = false;
    }

    void OnButtonClicked(int index)
    {
        if (buttons.Any(x => !IsInstanceValid(x)))
            return;

        Evolution selected = evolutions[index];
        PlayerManager.my.evolution = selected;
        if (!NetworkManager.Singleplayer)
        {
            // TODO: implement multiplayer
            // Client.instance.SendMessage(MessageType.Evolution, new EvolutionMessage(selected));
        }

        Hide();
    }
}