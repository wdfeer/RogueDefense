using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;
using System.Linq;

public static class NetworkManager
{
    public static int PlayerCount => 1 + (Singleplayer ? 0 : Client.instance.others.Count);
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
    public static void NetStop()
    {
        active = false;
        if (NetworkManager.mode == NetMode.Server)
        {
            Server.instance.Stop();
            Server.instance = new Server();
        }
        Client.client = null;
        Client.instance = new Client();
    }
    public static void Poll()
    {
        if (mode == NetMode.Server) Server.instance.Poll();
        Client.instance.Poll();
    }
}
public partial class UserData
{
    public int id;
    public string name;
    public int ability;
    public int[] upgradePoints;
    public static string UpgradePointsAsString(int[] upgradePoints) => String.Join("/", upgradePoints.Select(x => x.ToString()));
    public string UpgradePointsAsString() => UpgradePointsAsString(upgradePoints);
    public static int[] UpgradePointsFromString(string str) => str.Split("/").Select(x => int.Parse(x)).ToArray();
    public void SetUpgradePointsFromString(string str) => upgradePoints = UpgradePointsFromString(str);
    public UserData(int id, string name, int ability, int[] upgradePoints)
    {
        this.id = id;
        this.name = name;
        this.ability = ability;
        this.upgradePoints = upgradePoints;
    }
}
public enum NetMode
{
    Singleplayer,
    Server,
    Client
}
