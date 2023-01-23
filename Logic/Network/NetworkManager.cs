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
    }
    public static string connectingAddress;
    public static int connectingPort;
    public static string URL => $"wss://{connectingAddress}:{connectingPort}";
    public static void ConnectClient()
    {
        Client.client.ConnectToUrl(URL);
    }
}
public class UserData
{
    public int id;
    public string name;
    public UserData(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
public enum NetMode
{
    Singleplayer,
    Server,
    Client
}
