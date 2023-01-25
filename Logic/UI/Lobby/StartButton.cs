using Godot;
using System;

public class StartButton : Button
{
    public override async void _Pressed()
    {
        Client.instance.SendMessage(Client.MessageType.StartGame, new string[0]);
        ToSignal(GetTree().CreateTimer(1), "timeout").OnCompleted(ChangeLocalScene);
    }
    public void ChangeLocalScene()
    {
        Client.instance.ProcessMessage(Client.MessageType.StartGame, new string[0]);
    }
}
