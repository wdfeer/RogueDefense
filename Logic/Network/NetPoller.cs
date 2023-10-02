using Godot;
using System;

public partial class NetPoller : Node
{
    public override void _Process(double delta)
    {
        if (!NetworkManager.Singleplayer && NetworkManager.active)
            NetworkManager.Poll();
    }
}
