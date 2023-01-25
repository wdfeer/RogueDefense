using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;

public class Lobby : Control
{
    [Export]
    public PackedScene userDataScene;
    public static Lobby Instance => GDScript.IsInstanceValid(instance) ? instance : null;
    private static Lobby instance;
    public override void _Ready()
    {
        instance = this;

        (GetNode("PlayerList/MyData/Container/Name") as Label).Text = Player.myName;
        if (NetworkManager.mode == NetMode.Client)
            (GetNode("StartButton") as Button).Disabled = true;

        NetworkManager.NetStart();
    }
    Dictionary<int, PlayerData> userDisplayNodes = new Dictionary<int, PlayerData>();
    public void AddUser(UserData data)
    {
        var node = userDataScene.Instance() as PlayerData;
        node.SetName(data.name);
        userDisplayNodes.Add(data.id, node);
        GetNode("PlayerList").AddChild(node);
    }
    public void RemoveUser(int id)
    {
        if (userDisplayNodes.ContainsKey(id))
        {
            userDisplayNodes[id].QueueFree();
            userDisplayNodes.Remove(id);
        }
    }


    public override void _Process(float delta)
    {
        if (NetworkManager.mode == NetMode.Server) Server.instance.Poll();
        Client.instance.Poll();
    }
}
