using Godot;

namespace RogueDefense.Logic.UI.InGame;

public partial class MainMenuButton : Button
{
    public override void _Pressed()
    {
        GetTree().Paused = false;
        Game.instance.GoToMainMenu();
    }
}