using Godot;
using System;

public partial class MainMenuButton : Button
{
    public override void _Pressed()
    {
        if (!NetworkManager.Singleplayer)
        {
            NetworkManager.NetStop();
        }
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu/MainMenu.tscn");
        GetTree().Paused = false;
    }
}
