using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;

public class NetworkManager : Node
{
    public static NetMode mode = NetMode.Singleplayer;
    public static void NetStart()
    {
        if (NetworkManager.mode == NetMode.Server)
        {
            Server.instance.Start();
            Client.address = "localhost";
            Client.port = Server.PORT;
            Client.instance.Start();
        }
        else if (NetworkManager.mode == NetMode.Client)
        {
            Client.instance.Start();
        }
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
