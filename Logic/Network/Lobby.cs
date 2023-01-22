using Godot;
using RogueDefense;
using System;

public class Lobby : Control
{
    [Export]
    public PackedScene netManagerScene;
    public override void _Ready()
    {
        if (NetworkManager.mode == NetMode.Server)
        {
            NetworkManager networkManager = netManagerScene.Instance() as NetworkManager;
            AddChild(networkManager);
            networkManager.Initialize();

            (GetNode("PlayerList/MyData/Container/Name") as Label).Text = Player.myName;
        }
    }
}
