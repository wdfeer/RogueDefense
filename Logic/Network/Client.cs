using Godot;
using RogueDefense;
using System;
using System.Collections.Generic;

public class Client : Node
{
    public static Client instance = new Client();
    public static WebSocketClient client = new WebSocketClient();
    public static int myId;
    public void Start(string address)
    {
        client.Connect("connection_closed", this, "_closed");
        client.Connect("connection_error", this, "_closed");
        client.Connect("connection_established", this, "_connected");
        client.Connect("data_received", this, "_on_data");
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
                    var data = args[i].Split(";");
                    users.Add(new UserData(data[0].ToInt(), data[1]));
                }
                break;
            case MessageType.Register:
                users.Add(new UserData(args[0].ToInt(), args[1]));
                break;
            case MessageType.Unregister:
                users.Remove(users.Find(x => x.id == args[0].ToInt()));
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

    public override void _Process(float delta)
    {
        client.Poll();
    }
}