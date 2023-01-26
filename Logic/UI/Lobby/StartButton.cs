using Godot;
using System;

public class StartButton : Button
{
    public override void _Pressed()
    {
        if (NetworkManager.mode != NetMode.Server)
            return;

        Server.instance.SendMessage(MessageType.StartGame, new string[0]);
    }
}
