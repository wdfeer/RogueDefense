using Godot;
using System;

public class NetPoller : Node
{
    public override void _Process(float delta)
    {
        if (!NetworkManager.Singleplayer)
            NetworkManager.Poll();
    }
}
