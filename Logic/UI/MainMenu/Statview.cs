using RogueDefense.Logic.Save;

namespace RogueDefense.Logic.UI.MainMenu;

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
            SingleplayerHighscore.Text += SaveManager.user.highscoreSingleplayer.ToString();
            MultiplayerHighscore.Text += SaveManager.user.highscoreMultiplayer.ToString();
            GameCounter.Text += SaveManager.user.gameCount.ToString();
            KillCounter.Text += SaveManager.user.killCount.ToString();
        });
    }
}