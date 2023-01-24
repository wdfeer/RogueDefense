using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;

public class Client : Node
{
    public static string address;
    public static int port;
    public static string URL => $"ws://{address}:{port}";
    public static Client instance = new Client();
    public static WebSocketClient client;
    public static int myId;
    public void Start()
    {
        client = new WebSocketClient();
        client.Connect("connection_closed", this, "Closed");
        client.Connect("connection_error", this, "Closed");
        client.Connect("connection_established", this, "Connected");
        client.Connect("data_received", this, "OnData");
        GD.Print($"Trying to connect to {URL}");
        var err = client.ConnectToUrl(URL);
        if (err != Error.Ok)
        {
            GD.PrintErr("Unable to start client");
            SetProcess(false);
        }
    }
    public List<UserData> users = new List<UserData>();
    public void Connected(string protocol = "")
    {
        GD.Print("Client connected!");
    }
    public void Closed(bool wasCleanClose = false) { }
    public void OnData()
    {
        string data = client.GetPeer(1).GetPacket().GetStringFromUTF8();
        MessageType type = (MessageType)data[0];
        ProcessMessage(type, data.Substring(1).Split(' '));
    }
    void ProcessMessage(MessageType type, string[] args)
    {
        switch (type)
        {
            case MessageType.FetchLobby:
                SendMessage(MessageType.Register, new string[] { args[0], Player.myName });
                myId = args[0].ToInt();
                for (int i = 1; i < args.Length; i++)
                {
                    var strs = args[i].Split(";");
                    UserData data = new UserData(strs[0].ToInt(), strs[1]);
                    users.Add(data);
                    if (Lobby.Instance != null)
                    {
                        Lobby.Instance.AddUser(data);
                    }
                }
                break;
            case MessageType.Register:
                UserData d = new UserData(args[0].ToInt(), args[1]);
                users.Add(d);
                if (Lobby.Instance != null)
                {
                    Lobby.Instance.AddUser(d);
                }
                break;
            case MessageType.Unregister:
                int id = args[0].ToInt();
                users.Remove(users.Find(x => x.id == id));
                if (Lobby.Instance != null)
                {
                    Lobby.Instance.RemoveUser(id);
                }
                break;
            default:
                break;
        }
    }
    public enum MessageType
    {
        FetchLobby = 'i',
        Register = 'r',
        Unregister = 'R'
    }
    void Broadcast(string data) => client.PutPacket(System.Text.Encoding.UTF8.GetBytes(data));
    void SendMessage(MessageType type, string[] args)
    {
        Broadcast($"{type}" + String.Join(" ", args));
    }

    public void Poll()
    {
        client.Poll();
    }
}