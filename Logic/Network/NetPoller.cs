using Godot;
using System;

public partial class NetPoller : Node
{
    public override void _Process(float delta)
    {
        if (!NetworkManager.Singleplayer && NetworkManager.active)
            NetworkManager.Poll();
    }
}
