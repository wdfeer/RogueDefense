using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;

public static class NetworkManager
{
    public static bool Singleplayer => mode == NetMode.Singleplayer;
    public static NetMode mode = NetMode.Singleplayer;
    public static bool active = false;
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
        active = true;
    }
    public static void Poll()
    {
        if (mode == NetMode.Server) Server.instance.Poll();
        Client.instance.Poll();
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
