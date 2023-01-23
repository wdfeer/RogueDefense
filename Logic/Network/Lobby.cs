using Godot;
using RogueDefense;
using System;

public class Lobby : Control
{
    public static Lobby Instance => GDScript.IsInstanceValid(instance) ? instance : null;
    private static Lobby instance;
    [Export]
    public PackedScene netManagerScene;
    public override void _Ready()
    {
        instance = this;

        (GetNode("PlayerList/MyData/Container/Name") as Label).Text = Player.myName;

        NetworkManager networkManager = netManagerScene.Instance() as NetworkManager;
        AddChild(networkManager);
        if (NetworkManager.mode == NetMode.Server)
        {
            Server.instance.Start();
        }
        else if (NetworkManager.mode == NetMode.Client)
        {
            (GetNode("StartButton") as Button).Disabled = true;
            NetworkManager.ConnectClient();
        }
    }
}
