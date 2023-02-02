using Godot;
using RogueDefense;
using System;

public class Highscores : VBoxContainer
{
    public Label SingleplayerHighscore => (Label)GetNode("HighscoreSP");
    public Label MultiplayerHighscore => (Label)GetNode("HighscoreMP");
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.01f), "timeout").OnCompleted(() =>
        {
            SingleplayerHighscore.Text += UserSaveData.highscoreSingleplayer.ToString();
            MultiplayerHighscore.Text += UserSaveData.highscoreMultiplayer.ToString();
        });
    }
}
