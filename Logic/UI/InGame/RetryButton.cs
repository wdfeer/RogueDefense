using Godot;
using System;

public class RetryButton : Button
{
    public override void _Ready()
    {
        if (NetworkManager.mode == NetMode.Client)
            Disabled = true;
    }
    public override void _Pressed()
    {
        if (NetworkManager.Singleplayer)
        {
            GetTree().ChangeScene("res://Scenes/Game.tscn");
            GetTree().Paused = false;
        }
        else if (NetworkManager.mode == NetMode.Server)
        {
            Server.instance.SendMessage(MessageType.Retry);
        }
    }
}
