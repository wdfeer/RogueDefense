using Godot;

public partial class MainMenuButton : Button
{
    public override void _Pressed()
    {
        GetTree().Paused = false;
        Game.instance.GoToMainMenu();
    }
}
