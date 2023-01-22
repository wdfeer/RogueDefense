using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;

public class NetworkManager : Node
{
    public static NetMode mode = NetMode.Singleplayer;
    public static NetworkManager instance;

    public override void _Ready()
    {
        instance = this;
        GetTree().Connect("network_peer_connected", this, "PlayerConnected");
        GetTree().Connect("network_peer_disconnected", this, "PlayerDisconnected");
    }
    private static NetworkedMultiplayerENet net;
    public void InitializeServer()
    {
        users = new List<UserData>() { new UserData() { id = 1, name = Player.myName } };

        net = new NetworkedMultiplayerENet();
        net.CreateServer(7777, 7);
        GetTree().NetworkPeer = net;
    }
    public List<UserData> users;
    public void PlayerConnected(int id)
    {
        GD.Print($"Connection received, id: {id}");
        users.Add(new UserData() { id = id });
    }
    public void PlayerDisconnected(int id)
    {
        GD.Print($"Connection revoked, id: {id}");
        users.Remove(users.Find(x => x.id == id));
    }
    public static string connectingAddress;
    public static int connectingPort;
    public void ClientConnect()
    {
        net = new NetworkedMultiplayerENet();
        net.CreateClient(connectingAddress, connectingPort);
        GetTree().NetworkPeer = net;
        RpcId(1, "SetUsername", Player.myName);
    }
    [Remote]
    public void SetUsername(string name)
    {
        int sender = GetTree().GetRpcSenderId();
        users.Find(x => x.id == sender).name = name;
        GD.Print($"Set name {name} for user {sender}");
    }
}
public class UserData
{
    public int id;
    public string name;
}
public enum NetMode
{
    Singleplayer,
    Server,
    Client
}
