using Godot;
using System;

public class MainMenuButton : Button
{
    public override void _Pressed()
    {
        if (!NetworkManager.Singleplayer)
        {
            NetworkManager.NetStop();
        }
        GetTree().ChangeScene("res://Scenes/MainMenu/MainMenu.tscn");
        GetTree().Paused = false;
    }
}
