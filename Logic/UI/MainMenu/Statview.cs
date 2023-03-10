using Godot;
using RogueDefense;
using System;

public class Statview : VBoxContainer
{
    public Label SingleplayerHighscore => (Label)GetNode("HighscoreSP");
    public Label MultiplayerHighscore => (Label)GetNode("HighscoreMP");
    public Label GameCounter => (Label)GetNode("GameCount");
    public Label KillCounter => (Label)GetNode("KillCount");
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.01f), "timeout").OnCompleted(() =>
        {
            SingleplayerHighscore.Text += UserSaveData.highscoreSingleplayer.ToString();
            MultiplayerHighscore.Text += UserSaveData.highscoreMultiplayer.ToString();
            GameCounter.Text += UserSaveData.gameCount.ToString();
            KillCounter.Text += UserSaveData.killCount.ToString();
        });
    }
}
