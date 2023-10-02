using Godot;
using RogueDefense;
using System;

public partial class Statview : VBoxContainer
{
    public Label SingleplayerHighscore => (Label)GetNode("HighscoreSP");
    public Label MultiplayerHighscore => (Label)GetNode("HighscoreMP");
    public Label GameCounter => (Label)GetNode("GameCount");
    public Label KillCounter => (Label)GetNode("KillCount");
    public override void _Ready()
    {
        ToSignal(GetTree().CreateTimer(0.01f), "timeout").OnCompleted(() =>
        {
            SingleplayerHighscore.Text += SaveData.highscoreSingleplayer.ToString();
            MultiplayerHighscore.Text += SaveData.highscoreMultiplayer.ToString();
            GameCounter.Text += SaveData.gameCount.ToString();
            KillCounter.Text += SaveData.killCount.ToString();
        });
    }
}
